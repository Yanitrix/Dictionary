using Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Repository;

namespace Persistence.Repository
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        private bool disposed;

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                context.Dispose();
            }

            disposed = true;
        }

        protected readonly DatabaseContext context;
        protected readonly DbSet<T> repo;

        protected RepositoryBase(DatabaseContext context)
        {
            this.context = context;
            repo = context.Set<T>();
        }

        protected T GetOne(Expression<Func<T, bool>> condition, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = repo;

            if (include != null)
            {
                query = include(query);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.FirstOrDefault(condition);
        }

        protected IEnumerable<R> Get<R>(Expression<Func<T, bool>> condition, Expression<Func<T, R>> mapper,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = repo;

            if (include != null)
            {
                query = include(query);
            }

            query = query.Where(condition);

            if (orderBy != null)
            {
                return orderBy(query).Select(mapper);
            }

            return query.Select(mapper).ToList();
        }

        protected IEnumerable<T> All(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = repo;

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.ToList();
        }

        public virtual void Create(T entity)
        {
            repo.Add(entity);
            context.SaveChanges();
        }

        public virtual void CreateRange(params T[] entities)
        {
            repo.AddRange(entities);
            context.SaveChanges();
        }

        public virtual IEnumerable<T> All()
        {
            return repo.ToList();
        }

        public virtual T GetOne(Expression<Func<T, bool>> condition)
        {
            return repo.FirstOrDefault(condition);
        }

        public virtual IEnumerable<R> Get<R>(Expression<Func<T, bool>> condition, Expression<Func<T, R>> mapper)
        {
            return repo.Where(condition).Select(mapper).ToList();
        }

        public virtual void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            repo.Remove(entity);
            context.SaveChanges();
        }

        public bool Exists(Expression<Func<T, bool>> condition)
        {
            return repo.Any(condition);
        }
    }
}