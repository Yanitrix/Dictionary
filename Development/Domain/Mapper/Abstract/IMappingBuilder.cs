using System;

namespace Domain.Mapper.Abstract
{
    public interface IMappingBuilder<T , R>
    {
        internal Func<T, R> Constructor { get; }

        internal Func<T, R, R> Mapping { get; }

        /// <summary>
        /// Enforce the way destination object should be instantiated. If this method is not used, then default constructor will be used.
        /// </summary>
        /// <param name="constructor">The function used to instantiade a new destination object from source object.</param>
        public void InstantiateUsing(Func<T, R> constructor);

        /// <summary>
        /// Provides instances of both source object and destination object. The destination object should not be created within the method.
        /// </summary>
        /// <param name="mappingFunction">Function used to map from source type to destination type.</param>
        public IMappingBuilder<T, R> RegisterMapping(Func<T, R, R> mappingFunction);
    }
}
