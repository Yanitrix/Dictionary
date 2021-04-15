using Domain.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Database;
using Persistence.Repository;

namespace Persistence
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(
                    config.GetConnectionString("ConnectionString")));

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<DatabaseContext>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDictionaryRepository, DictionaryRepository>();
            services.AddTransient<IEntryRepository, EntryRepository>();
            services.AddTransient<IFreeExpressionRepository, FreeExpressionRepository>();
            services.AddTransient<IMeaningRepository, MeaningRepository>();
            services.AddTransient<ILanguageRepository, LanguageRepository>();
            services.AddTransient<IWordRepository, WordRepository>();

            return services;
        }
    }
}
