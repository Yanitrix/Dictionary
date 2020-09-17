using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Api.Service
{
    public interface IRepository<T> : IDisposable
    {
        public IEnumerable<T> All();

        public T Create(T entity);

        public T Delete(T entity);

        public T Update(T entity);

        public T GetOne(Expression<Func<T, bool>> condition);

        public IEnumerable<R> Get<R>(Expression<Func<T, bool>> condition, Expression<Func<T, R>> mapper);

    }
}