using System;
using Domain.Mapper.Abstract;

namespace Domain.Mapper
{
    public class MappingBuilder<T, R> : IMappingBuilder<T, R>
    {
        private Func<T, R> constructor;
        private Func<T, R, R> mapping;

        Func<T, R> IMappingBuilder<T, R>.Constructor => constructor;

        Func<T, R, R> IMappingBuilder<T, R>.Mapping => mapping;

        public void InstantiateUsing(Func<T, R> constructor)
        {
            this.constructor = constructor;
        }

        public IMappingBuilder<T, R> RegisterMapping(Func<T, R, R> mappingFunction)
        {
            this.mapping = mappingFunction;
            return this;
        }
    }
}
