using Api.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Service
{
    public interface IService<T> 
    {
        public ValidationDictionary TryAdd(T entity);
        
        public ValidationDictionary TryUpdate(T entity);

        public bool IsValid(T entity);
    }
}
