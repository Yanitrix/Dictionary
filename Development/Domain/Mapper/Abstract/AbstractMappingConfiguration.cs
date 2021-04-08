using System;
using System.Collections.Generic;
using Domain.Mapper.Abstract;

namespace Domain.Mapper
{
    public abstract class AbstractMappingConfiguration
    {
        private Dictionary<(Type, Type), object> builders = new();
        private readonly IMapper mapper;

        protected AbstractMappingConfiguration()
        {
            mapper = new Mapper(this);
        }

        public IMapper CreateMapper() => this.mapper;

        protected IMappingBuilder<T, R> RegisterMapping<T, R>(Func<T, R, R> mappingFunction)
        {
            IMappingBuilder<T, R> builder = new MappingBuilder<T, R>();
            var typeIn = typeof(T);
            var typeOut = typeof(R);
            builders.Add((typeIn, typeOut), builder);
            builder.RegisterMapping(mappingFunction);
            return builder;
        }

        internal IMappingBuilder<T, R> Builder<T, R>()
        {
            var found = builders[(typeof(T), typeof(R))];
            if (found == null)
                throw new Exception();//message
            return (IMappingBuilder<T, R>)found;
        }

        protected R Map<T, R>(T source)
        {
            return mapper.Map<T, R>(source);
        }

        protected R Map<T, R>(T source, R destination)
        {
            return mapper.Map(source, destination);
        }
    }
}
