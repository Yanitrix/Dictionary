using Data.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Tests
{
    public class DbContextTestBase : IDisposable
    {
        protected DatabaseContext context;

        public DbContextTestBase()
        {
            initializeDatabase();
        }
        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        protected void initializeDatabase()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            this.context = new DatabaseContext(options);

            context.Database.EnsureCreated();
        }
    }
}
