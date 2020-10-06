using Api.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Service
{
    public interface IService<T>
    {
        public IValidationDictionary TryAdd(T entity);
        
        public IValidationDictionary TryUpdate(T entity);

        public IValidationDictionary IsValid(T entity);
    }
}
