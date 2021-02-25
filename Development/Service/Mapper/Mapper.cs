using System;

namespace Service.Mapper
{
    public class Mapper : IMapper
    {
        private readonly AbstractMappingConfiguration configuration;

        public Mapper(AbstractMappingConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private R Construct<R>()
        {
            var type = typeof(R);
            //check if has default constructor
            var existing = type.GetConstructor(Type.EmptyTypes);
            if (existing == null)
                throw new Exception("parametereless ctor does not exist");//make a valid exception here
            object instance = Activator.CreateInstance(type);
            return (R)instance;
        }

        public R Map<T, R>(T source)
        {
            var builder = configuration.Builder<T, R>();
            //check if has a construct function
            R result = builder.Constructor == null ? Construct<R>() : builder.Constructor(source);
            return builder.Mapping(source, result);
        }

        public R Map<T, R>(T source, R destination)
        {
            var builder = configuration.Builder<T, R>();
            return builder.Mapping(source, destination);
        }
    }
}
