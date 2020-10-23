using Service.Repository;
using Data.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Service.Tests.Service
{
    public class WordServiceTests : UowTestBase
    {
        IService<Word> service;
        Mock<IWordRepository> _wordRepo = new Mock<IWordRepository>();
        Mock<ILanguageRepository> _langRepo = new Mock<ILanguageRepository>();

        public WordServiceTests()
        {
            wordRepo = _wordRepo;
            langRepo = _langRepo;
            service = new WordService(this.uow.Object, VMoq.Instance<Word>());
        }

        [Fact]
        public void TryAdd_LanguageExists_PropertiesGood_ShouldAdd()
        {
            _wordRepo.Setup(_ => _.GetByValue(It.IsAny<String>(), It.IsAny<bool>())).Returns(Enumerable.Empty<Word>()); //no duplicated words
            _langRepo.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(true); //source language exists

            var entity = new Word();

            var result = service.TryAdd(entity);

            _wordRepo.Verify(_ => _.Create(It.IsAny<Word>()), Times.Once);
            Assert.Empty(result);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void TryAdd_LanguageExists_AnotherWordWithSamePropertiesExist_ShouldReturnError()
        {
            var toReturn = new Word
            {
                Value = "value", //lowercase
                Properties = new WordPropertySet
                {
                    new WordProperty
                    {
                        Name = "name1 ",
                        Values = new StringSet("value2", "value1")
                    },

                    new WordProperty
                    {
                        Name = "name2",
                        Values = new StringSet("VAlue3", "value4")
                    },
                }
            };

            var entity = new Word
            {
                Value = "Value", //uppercase
                Properties = new WordPropertySet
                {
                    new WordProperty
                    {
                        Name = "name1",
                        Values = new StringSet("value1", "value2")
                    },

                    new WordProperty
                    {
                        Name = "NAme2",
                        Values = new StringSet("value3", "valuE4")
                    },
                }
            };

            _wordRepo.Setup(_ => _.GetByLanguageAndValue(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<bool>())).Returns(new Word[] { toReturn });
            _langRepo.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(true); //source language exists

            var result = service.TryAdd(entity);

            _wordRepo.Verify(_ => _.Create(It.IsAny<Word>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Duplicate", result.First().Key);
        }

        [Fact]
        public void TryAdd_PropertiesGood_LanguageDoesNotExist_ShouldReturnError()
        {
            _wordRepo.Setup(_ => _.GetByValue(It.IsAny<String>(), It.IsAny<bool>())).Returns(Enumerable.Empty<Word>()); //no duplicated words
            _langRepo.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(false); //source language doesnt exist

            var entity = new Word();
            var result = service.TryAdd(entity);

            _wordRepo.Verify(_ => _.Create(It.IsAny<Word>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Language not found", result.First().Key);
        }

        [Fact]
        public void TryAdd_NorLanguageNorPropertiesAreGood_ShouldReturnError()
        {
            _wordRepo.Setup(_ => _.GetByValue(It.IsAny<String>(), It.IsAny<bool>())).Returns(Enumerable.Empty<Word>());
            //actually it doesn't matter since when language is not found the method will be returned
            _langRepo.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(false); //source language doesnt exist

            var entity = new Word();
            var result = service.TryAdd(entity);

            _wordRepo.Verify(_ => _.Create(It.IsAny<Word>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Language not found", result.First().Key);
        }

        [Fact]
        public void TryUpdate_WordDoesNotExist_ReturnsError()
        {
            var entity = new Word
            {
                ID = 1,
                SourceLanguageName = "english"
            };

            _wordRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.ID))).Returns(false);

            var result = service.TryUpdate(entity);

            _wordRepo.Verify(_ => _.Update(It.IsAny<Word>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Entity does not exist", result.First().Key);
        }

        [Fact]
        public void TryUpdate_WordFound_EverythingGodd_ReturnsNoErrors()
        {
            var entity = new Word
            {
                ID = 2,
                SourceLanguageName = "english"
            };

            _wordRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.ID))).Returns(true);
            _wordRepo.Setup(_ => _.GetByValue(It.IsAny<String>(), It.IsAny<bool>())).Returns(Enumerable.Empty<Word>());
            _langRepo.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(true);

            var result = service.TryUpdate(entity);

            _wordRepo.Verify(_ => _.Update(It.IsAny<Word>()), Times.Once);
            Assert.Empty(result);
            Assert.True(result.IsValid);
        }
    }
}
