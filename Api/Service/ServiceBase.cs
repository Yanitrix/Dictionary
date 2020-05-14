using Dictionary_MVC.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    public abstract class ServiceBase<T> : IService<T>
    {
        protected readonly DatabaseContext context;

        public IValidationDictionary ValidationDictionary { get; set; } 

        public ServiceBase(DatabaseContext context)
        {
            this.context = context;
        }

        public abstract T Create(T entity);

        public abstract T Delete(T entity);

        public abstract IEnumerable<T> Get(Func<T, bool> condition);

        public abstract T GetOne(Func<T, bool> condition);

        public abstract T Update(T entity);

        public abstract bool IsValid(T entity);
    }
}