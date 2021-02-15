using System;

namespace Service.Mapper
{
    public interface IMappingContext
    {
        public Func<T, R> ResolveMappingFunction<T, R>();

        public void RegisterMapping<T, R>(Func<T, R> mappingFunction);
    }
}
