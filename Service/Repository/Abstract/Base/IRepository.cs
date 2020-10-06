using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Service.Repository
{
    public interface IRepository<T> : IDisposable
    {
        public IEnumerable<T> All();

        public T Create(T entity);

        public void CreateRange(params T[] entities);

        public T Delete(T entity);

        public T Update(T entity);

        public T GetOne(Expression<Func<T, bool>> condition);

        public IEnumerable<R> Get<R>(Expression<Func<T, bool>> condition, Expression<Func<T, R>> mapper);

        public void Detach(T entity);

        public bool Exists(Expression<Func<T, bool>> condition);
    }
}