using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Utils
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var congif = new MapperConfiguration(config =>
            {
                config.AddProfile(new MapperProfile());
            });

            IMapper mapper = congif.CreateMapper();
            return services.AddSingleton(mapper);
        }
    }
}
