﻿using Domain.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Database;
using Persistance.Repository;

namespace Persistance
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

            return services;
        }
    }
}
