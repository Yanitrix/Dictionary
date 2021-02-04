using System;
using System.Linq.Expressions;

namespace Data.Mapper
{
    public interface IMappingContext
    {
        public Func<T, R> ResolveMappingFunction<T, R>();

        public void RegisterMapping<T, R>(Func<T, R> mappingFunction);
    }
}
