
namespace Service.Mapper
{
    public class Mapper : IMapper
    {
        private readonly AbstractMappingConfiguration configuration;

        public Mapper(AbstractMappingConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public R Map<T, R>(T source)
        {
            return configuration.Context.ResolveMappingFunction<T, R>()(source);
        }
    }
}
