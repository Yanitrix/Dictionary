using Microsoft.Extensions.DependencyInjection;

namespace Data.Mapper
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddSimpleMapper<T>(this IServiceCollection services) where T : AbstractMappingConfiguration, new()
        {
            var config = new T();
            return services.AddSingleton(IMapper.CreateMapper(config));
        }
    }
}
