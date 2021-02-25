using Domain.Models;
using Moq;
using Domain.Repository;
using System;
using System.Linq;
using Xunit;
using Service.Mapper;
using Domain.Dto;

namespace Service.Tests.Service
{
    public class DictionaryServiceTests : UowTestBase
    {
        Mock<ILanguageRepository> _langRepo = new Mock<ILanguageRepository>();
        Mock<IDictionaryRepository> _dictRepo = new Mock<IDictionaryRepository>();

        IDictionaryService serviceUnderTests;

        public DictionaryServiceTests()
        {
            langRepo = _langRepo;
            dictRepo = _dictRepo;
            serviceUnderTests = new DictionaryService(this.uow.Object, this.mapper);
        }

        [Fact]
        public void TryAdd_LanguagesExistAndNotDuplicate_ShouldAddProperly()
        {
            _langRepo.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(true); //any language exists
            _dictRepo.Setup(_ => _.ExistsByLanguages(It.IsAny<String>(), It.IsAny<String>())).Returns(false); //dictionary is not a duplicate

            var dto = new CreateDictionary();
            var result = serviceUnderTests.Add(dto);

            _dictRepo.Verify(_ => _.Create(It.IsAny<Dictionary>()), Times.Once);
            Assert.Empty(result);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void TryAdd_AlreadyExists_ShouldReturnError()
        {
            _langRepo.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(true); //any language exists
            _dictRepo.Setup(_ => _.ExistsByLanguages(It.IsAny<String>(), It.IsAny<String>())).Returns(true); //dictionary is a duplicate

            var dto = new CreateDictionary();

            var result = serviceUnderTests.Add(dto);

            _dictRepo.Verify(_ => _.Create(It.IsAny<Dictionary>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Duplicate", result.First().Name);
        }

        [Fact]
        public void TryAdd_BothLanguagesDontExist_ShouldReturnError()
        {
            _langRepo.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(false); //languages dont exist
            _dictRepo.Setup(_ => _.ExistsByLanguages(It.IsAny<String>(), It.IsAny<String>())).Returns(false); //dictionary is not a duplicate 

            var dto = new CreateDictionary
            {
                LanguageIn = "in",
                LanguageOut = "out",
            };

            var result = serviceUnderTests.Add(dto);

            Assert.Single(result);
            Assert.Equal("Language does not exist.", result.First().Name);
        }

        [Fact]
        public void TryAdd_OneLanguageDoesNotExist_ShouldReturnError()
        {
            const String name = "testName";
            _langRepo.Setup(_ => _.ExistsByName(It.Is<String>(l => l == name))).Returns(false); // "testName" does not exist
            _langRepo.Setup(_ => _.ExistsByName(It.Is<String>(l => l != name))).Returns(true); // any other language exists
            _dictRepo.Setup(_ => _.ExistsByLanguages(It.IsAny<String>(), It.IsAny<String>())).Returns(false); //any dictionary does not exist

            var dto = new CreateDictionary
            {
                LanguageIn = "testName",
                LanguageOut = "out"
            };


            var result = serviceUnderTests.Add(dto);

            Assert.Single(result);
            Assert.Equal("Language does not exist.", result.First().Name);
        }

    }
}
