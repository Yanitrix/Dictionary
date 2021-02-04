using System;

namespace Data.Mapper
{
    public abstract class AbstractMappingConfiguration
    {
        public AbstractMappingConfiguration()
        {
            Context = new MappingContext();
        }

        public IMappingContext Context { get; }

        protected void RegisterMapping<T, R>(Func<T, R> mappingFunction) => Context.RegisterMapping(mappingFunction);
        protected Func<T, R> ResolveMappingFunction<T, R>() => Context.ResolveMappingFunction<T, R>();
        
    }
}
