using Api.Service;
using Data.Models;
using Moq;
using Service.Service;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace Service.Tests.Service
{
    public class DictionaryServiceTests
    {
        Mock<ILanguageRepository> langRepo = new Mock<ILanguageRepository>();
        Mock<IDictionaryRepository> dictRepo = new Mock<IDictionaryRepository>();

        IService<Dictionary> service;

        public DictionaryServiceTests()
        {
            service = new DictionaryService(langRepo.Object, dictRepo.Object, VMoq.Instance<Dictionary>());
        }

        [Fact]
        public void TryUpdate_Impossible_AlwaysReturnsError()
        {
            langRepo.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(true); //any language exists
            dictRepo.Setup(_ => _.ExistsByLanguages(It.IsAny<String>(), It.IsAny<String>())).Returns(false); //dictionary is not a duplicate

            var entity = new Dictionary();
            var result = service.TryUpdate(entity);

            Assert.Single(result);
            Assert.Equal("Entity cannot be updated", result.First().Key);
        }

        [Fact]
        public void TryAdd_LanguagesExistAndNotDuplicate_ShouldAddProperly()
        {
            langRepo.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(true); //any language exists
            dictRepo.Setup(_ => _.ExistsByLanguages(It.IsAny<String>(), It.IsAny<String>())).Returns(false); //dictionary is not a duplicate

            var entity = new Dictionary();
            var result = service.TryAdd(entity);

            dictRepo.Verify(_ => _.Create(It.IsAny<Dictionary>()), Times.Once);
            Assert.Empty(result);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void TryAdd_AlreadyExists_ShouldReturnError()
        {
            langRepo.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(true); //any language exists
            dictRepo.Setup(_ => _.ExistsByLanguages(It.IsAny<String>(), It.IsAny<String>())).Returns(true); //dictionary is a duplicate

            var entity = new Dictionary();

            var result = service.TryAdd(entity);

            dictRepo.Verify(_ => _.Create(It.IsAny<Dictionary>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Duplicate", result.First().Key);
        }

        [Fact]
        public void TryAdd_BothLanguagesDontExist_ShouldReturnError()
        {
            langRepo.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(false); //languages dont exist
            dictRepo.Setup(_ => _.ExistsByLanguages(It.IsAny<String>(), It.IsAny<String>())).Returns(false); //dictionary is not a duplicate 

            var entity = new Dictionary
            {
                LanguageInName = "in",
                LanguageOutName = "out",
            };

            var restult = service.TryAdd(entity);

            Assert.Single(restult);
            Assert.Equal("Language not found", restult.First().Key);
        }

        [Fact]
        public void TryAdd_OneLanguageDoesNotExist_ShouldReturnError()
        {
            const String name = "testName";
            langRepo.Setup(_ => _.ExistsByName(It.Is<String>(l => l == name))).Returns(false); // "testName" does not exist
            langRepo.Setup(_ => _.ExistsByName(It.Is<String>(l => l != name))).Returns(true); // any other language exists
            dictRepo.Setup(_ => _.ExistsByLanguages(It.IsAny<String>(), It.IsAny<String>())).Returns(false); //any dictionary does not exist

            var entity = new Dictionary
            {
                LanguageInName = "testName",
                LanguageOutName = "out"
            };


            var result = service.TryAdd(entity);

            Assert.Single(result);
            Assert.Equal("Language not found", result.First().Key);
        }

    }
}
