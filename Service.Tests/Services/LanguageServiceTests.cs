using Api.Service;
using Api.Service.Validation;
using Data.Models;
using Data.Tests;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Service.Tests.Services
{
    public class LanguageServiceTests : DbContextTestBase
    {
        private LanguageService service;

        public LanguageServiceTests()
        {
            service = new LanguageService(context, new LanguageValidator());
            service.ValidationDictionary = new ValidationDictionary();
            putData();
        }

        private void putData()
        {
            var list = new List<Language>
            {
                new Language{Name = "japanese"},
                new Language{Name = "german"},
                new Language{Name = "english"},
            };

            service.CreateRange(list.ToArray());
        }

        [Fact]
        public void GetByName_StringNull_ReturnsNull()
        {
            String name = null;
            var found = service.GetByName(name);

            Assert.Null(found);
        }

        [Theory]
        [InlineData("ssds")]
        [InlineData("polish")]
        [InlineData("russian")]
        [InlineData("śöäąć")]
        public void GetByName_NameDoesNotExist_ReturnsNull(String name)
        {
            var found = service.GetByName(name);
            Assert.Null(found);
        }

        [Theory]
        [InlineData("japanese")]
        [InlineData("german")]
        [InlineData("english")]
        public void GetByName_NameExists_ReturnsEntity(String name)
        {
            var found = service.GetByName(name);

            Assert.NotNull(found);
            Assert.Equal(typeof(Language), found.GetType());
            Assert.Equal(name, found.Name);
        }

        [Theory]
        [InlineData("japanese")]
        [InlineData("german")]
        public void IsReadyToAdd_AlreadyExists_ShouldBeFalse(String name)
        {
            var entity = new Language { Name = name };
            var result = service.IsReadyToAdd(entity);

            Assert.False(result);
        }

        [Theory]
        [InlineData("russian")]
        [InlineData("urdu")]
        public void IsReadyToAdd_DoesNotExist_ShouldBeTrue(String name)
        {
            var entity = new Language { Name = name };
            var result = service.IsReadyToAdd(entity);

            Assert.True(result);
        }
    }
}
