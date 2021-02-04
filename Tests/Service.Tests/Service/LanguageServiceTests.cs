using Data.Repository;
using Data.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Service.Tests.Service
{
    public class LanguageServiceTests : UowTestBase
    {
        IService<Language> service;
        Mock<ILanguageRepository> langRepoMock = new Mock<ILanguageRepository>();


        public LanguageServiceTests()
        {
            langRepo = langRepoMock;
            service = new LanguageService(this.uow.Object);
        }

        [Fact]
        public void TryAdd_LanguageExists_ReturnsError()
        {
            const string name = "name";
            langRepoMock.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(true);

            var lang = new Language
            {
                Name = name
            };

            var result = service.Add(lang);

            Assert.Single(result);
            Assert.Equal("Duplicate", result.First().Key);
        }

        [Fact]
        public void TryAdd_LanguageDoesNotExist_AddsProperly()
        {
            IList<Language> repo = new List<Language>();

            langRepoMock.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(false);
            langRepoMock.Setup(_ => _.Create(It.IsAny<Language>())).Callback<Language>(x => repo.Add(x));

            var lang = new Language();
            var result = service.Add(lang);

            Assert.Empty(result);
            Assert.Single(repo);
        }
        
        [Fact]
        public void TryUpdate_Impossible_AlwaysReturnsError()
        {
            langRepoMock.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(false);

            var lang = new Language();
            var result = service.Update(lang);

            Assert.Single(result);
            Assert.Equal("Entity cannot be updated", result.First().Key);
        }
    }
}
