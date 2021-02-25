using Domain.Repository;
using Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Domain.Dto;

namespace Service.Tests.Service
{
    public class LanguageServiceTests : UowTestBase
    {
        ILanguageService service;
        Mock<ILanguageRepository> langRepoMock = new Mock<ILanguageRepository>();


        public LanguageServiceTests()
        {
            langRepo = langRepoMock;
            service = new LanguageService(this.uow.Object, this.mapper);
        }

        [Fact]
        public void TryAdd_LanguageExists_ReturnsError()
        {
            const string name = "name";
            langRepoMock.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(true);

            var dto = new CreateLanguage
            {
                Name = name
            };

            var result = service.Add(dto);

            Assert.Single(result);
            Assert.Equal("Duplicate", result.First().Name);
        }

        [Fact]
        public void TryAdd_LanguageDoesNotExist_AddsProperly()
        {
            IList<Language> repo = new List<Language>();

            langRepoMock.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(false);
            langRepoMock.Setup(_ => _.Create(It.IsAny<Language>())).Callback<Language>(x => repo.Add(x));

            var dto = new CreateLanguage();
            var result = service.Add(dto);

            Assert.Empty(result);
            Assert.Single(repo);
        }
    }
}
