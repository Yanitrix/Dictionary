using Domain.Models;
using Moq;
using Domain.Repository;
using System;
using System.Linq;
using Xunit;

namespace Service.Tests.Service
{
    public class DictionaryServiceTests : UowTestBase
    {
        Mock<ILanguageRepository> _langRepo = new Mock<ILanguageRepository>();
        Mock<IDictionaryRepository> _dictRepo = new Mock<IDictionaryRepository>();

        IService<Dictionary> service;

        public DictionaryServiceTests()
        {
            langRepo = _langRepo;
            dictRepo = _dictRepo;
            service = new DictionaryService(this.uow.Object);
        }

        [Fact]
        public void TryUpdate_Impossible_AlwaysReturnsError()
        {
            _langRepo.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(true); //any language exists
            _dictRepo.Setup(_ => _.ExistsByLanguages(It.IsAny<String>(), It.IsAny<String>())).Returns(false); //dictionary is not a duplicate

            var entity = new Dictionary();
            var result = service.Update(entity);

            Assert.Single(result);
            Assert.Equal("Entity cannot be updated", result.First().Name);
        }

        [Fact]
        public void TryAdd_LanguagesExistAndNotDuplicate_ShouldAddProperly()
        {
            _langRepo.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(true); //any language exists
            _dictRepo.Setup(_ => _.ExistsByLanguages(It.IsAny<String>(), It.IsAny<String>())).Returns(false); //dictionary is not a duplicate

            var entity = new Dictionary();
            var result = service.Add(entity);

            _dictRepo.Verify(_ => _.Create(It.IsAny<Dictionary>()), Times.Once);
            Assert.Empty(result);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void TryAdd_AlreadyExists_ShouldReturnError()
        {
            _langRepo.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(true); //any language exists
            _dictRepo.Setup(_ => _.ExistsByLanguages(It.IsAny<String>(), It.IsAny<String>())).Returns(true); //dictionary is a duplicate

            var entity = new Dictionary();

            var result = service.Add(entity);

            _dictRepo.Verify(_ => _.Create(It.IsAny<Dictionary>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Duplicate", result.First().Name);
        }

        [Fact]
        public void TryAdd_BothLanguagesDontExist_ShouldReturnError()
        {
            _langRepo.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(false); //languages dont exist
            _dictRepo.Setup(_ => _.ExistsByLanguages(It.IsAny<String>(), It.IsAny<String>())).Returns(false); //dictionary is not a duplicate 

            var entity = new Dictionary
            {
                LanguageInName = "in",
                LanguageOutName = "out",
            };

            var restult = service.Add(entity);

            Assert.Single(restult);
            Assert.Equal("Language does not exist.", restult.First().Name);
        }

        [Fact]
        public void TryAdd_OneLanguageDoesNotExist_ShouldReturnError()
        {
            const String name = "testName";
            _langRepo.Setup(_ => _.ExistsByName(It.Is<String>(l => l == name))).Returns(false); // "testName" does not exist
            _langRepo.Setup(_ => _.ExistsByName(It.Is<String>(l => l != name))).Returns(true); // any other language exists
            _dictRepo.Setup(_ => _.ExistsByLanguages(It.IsAny<String>(), It.IsAny<String>())).Returns(false); //any dictionary does not exist

            var entity = new Dictionary
            {
                LanguageInName = "testName",
                LanguageOutName = "out"
            };


            var result = service.Add(entity);

            Assert.Single(result);
            Assert.Equal("Language does not exist.", result.First().Name);
        }

    }
}
