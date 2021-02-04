using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Data.Mapper
{
    public class MappingContext : IMappingContext
    {
        private Dictionary<(Type, Type), Delegate> mappingFunctions = new();

        public void RegisterMapping<T, R>(Func<T, R> function)
        {
            var typeIn = typeof(T);
            var typeOut = typeof(R);

            mappingFunctions[(typeIn, typeOut)] = function;
        }

        public Func<T, R> ResolveMappingFunction<T, R>()
        {

            var typeIn = typeof(T);
            var typeOut = typeof(R);
            var resolved = mappingFunctions.GetValueOrDefault((typeIn, typeOut));
            if (resolved == null)
                throw new KeyNotFoundException($"No mapping function for types \"{typeIn}\" and \"{typeOut}\" was registered.");
            return resolved as Func<T, R>;
        }
    }
}
