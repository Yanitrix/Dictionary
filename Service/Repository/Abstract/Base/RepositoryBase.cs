﻿using Data.Database;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Api.Service
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

        public RepositoryBase(DatabaseContext context)
        {
            this.context = context;
            repo = context.Set<T>();
        }

        public IEnumerable<T> All()
        {
            return repo.ToList();
        }

        public virtual T Create(T entity)
        {
            repo.Add(entity);
            context.SaveChanges();
            return entity;
        }

        public virtual void CreateRange(params T[] entities)
        {
            repo.AddRange(entities);
            context.SaveChanges();
        }

        public virtual T GetOne(Expression<Func<T, bool>> condition)
        {
            return repo.FirstOrDefault(condition);
        }

        public virtual IEnumerable<R> Get<R>(Expression<Func<T, bool>> condition, Expression<Func<T, R>> mapper)
        {
            return repo.Where(condition).Select(mapper).ToList();
        }

        public virtual T Update(T entity)
        {
            repo.Update(entity);
            context.SaveChanges();
            return entity;
        }

        public virtual T Delete(T entity)
        {
            repo.Remove(entity);
            context.SaveChanges();
            return entity;
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

            if(orderBy != null)
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

    }
}