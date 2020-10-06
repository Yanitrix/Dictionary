using Data.Models;
using Data.Tests;
using System;
using System.Collections.Generic;
using Xunit;
using Service.Repository;

namespace Service.Tests.Repositories
{
    public class WordPropertyRepositoryTests : DbContextTestBase
    {
        public IWordPropertyRepository repository;

        public WordPropertyRepositoryTests()
        {
            repository = new WordPropertyRepository(this.context);
        }

        [Fact]
        public void test()
        {
            var wp = new WordProperty
            {
                Name = "hstus",
                Values = new HashSet<String> { "pan1", "pan2" },
            };

            repository.Create(wp);
        }
    }
}
