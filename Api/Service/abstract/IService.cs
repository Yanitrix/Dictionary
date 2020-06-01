using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    public interface IService<T>
    {
        public IValidationDictionary ValidationDictionary { get; set; }

        public bool IsValid(T entity);

        public bool IsReadyToAdd(T entity);

        public bool IsReadyToUpdate(T entity);

        public IEnumerable<T> All();

        public T Create(T entity);

        public T Delete(T entity);

        public T Update(T entity);

        public T GetOne(Func<T, bool> condition);

        public IEnumerable<T> Get(Func<T, bool> condition);
    }
}