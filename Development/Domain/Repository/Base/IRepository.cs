using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Repository
{
    public interface IRepository<T, K> : IDisposable
    {
        public IEnumerable<T> All();

        public void Create(T entity);

        public void CreateRange(params T[] entities);

        public void Delete(T entity);

        public void Update(T entity);

        public T GetByPrimaryKey(K pkey);
        
        public T GetOne(Expression<Func<T, bool>> condition);

        public IEnumerable<R> Get<R>(Expression<Func<T, bool>> condition, Expression<Func<T, R>> mapper);

        public bool Exists(Expression<Func<T, bool>> condition);

        public bool ExistsByPrimaryKey(K pkey);
        
        
    }
}