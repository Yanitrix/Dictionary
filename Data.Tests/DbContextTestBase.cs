using Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Tests
{
    public class DbContextTestBase : IDisposable
    {
        protected DatabaseContext context;

        private DbContextOptions<DatabaseContext> options;

        public DbContextTestBase()
        {
            initializeDatabase();
        }
        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        protected void changeContext()
        {
            this.context.Dispose();
            this.context = new DatabaseContext(options);
        }

        private void initializeDatabase()
        {
            this.context?.Database.EnsureDeleted();
            this.context?.Dispose();

            var services = new ServiceCollection().AddEntityFrameworkSqlServer().BuildServiceProvider();

            options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=dictionary_{Guid.NewGuid()};Trusted_Connection=True").
                UseInternalServiceProvider(services).
                Options;

            this.context = new DatabaseContext(options);
            this.context.Database.Migrate();
        }
    }
}
