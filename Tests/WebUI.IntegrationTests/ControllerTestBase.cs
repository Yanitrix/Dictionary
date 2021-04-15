using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace WebUI.IntegrationTests
{
    public class ControllerTestBase
    {
        protected readonly HttpClient client;

        public ControllerTestBase()
        {
            var factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builde =>
                {
                    builde.ConfigureServices(services =>
                    {
                        //services.RemoveAll(typeof(DatabaseContext));
                        //services.AddDbContext<DatabaseContext>(opt => opt.UseInMemoryDatabase("test_db"));

                        ReplaceCoreServices<DatabaseContext>(services, (p, o) =>
                        {
                            o.UseInMemoryDatabase("DB");
                        }, ServiceLifetime.Scoped);
                    });

                    builde.Configure(app =>
                    {
                        using (var scope = app.ApplicationServices.CreateScope())
                        {
                            var context = scope.ServiceProvider.GetService<DatabaseContext>();
                            context.Database.EnsureCreated();
                        }
                    });
                });

            client = factory.CreateClient();
        }

        //code copied from here https://github.com/dotnet/aspnetcore/issues/14955#issuecomment-535933760

        private static void ReplaceCoreServices<TContextImplementation>(IServiceCollection serviceCollection,
                                        Action<IServiceProvider, DbContextOptionsBuilder> optionsAction,
                                        ServiceLifetime optionsLifetime) where TContextImplementation : DbContext
        {
            serviceCollection.Add(new ServiceDescriptor(typeof(DbContextOptions<TContextImplementation>),
                                  (IServiceProvider p) => DbContextOptionsFactory<TContextImplementation>(p, optionsAction), optionsLifetime));
            serviceCollection.Add(new ServiceDescriptor(typeof(DbContextOptions),
                                  (IServiceProvider p) => p.GetRequiredService<DbContextOptions<TContextImplementation>>(), optionsLifetime));
        }

        private static DbContextOptions<TContext> DbContextOptionsFactory<TContext>(IServiceProvider applicationServiceProvider,
                                                   Action<IServiceProvider, DbContextOptionsBuilder> optionsAction) where TContext : DbContext
        {
            DbContextOptionsBuilder<TContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<TContext>(
                new DbContextOptions<TContext>(new Dictionary<Type, IDbContextOptionsExtension>()));
            dbContextOptionsBuilder.UseApplicationServiceProvider(applicationServiceProvider);
            optionsAction?.Invoke(applicationServiceProvider, dbContextOptionsBuilder);
            return dbContextOptionsBuilder.Options;
        }
    }
}
