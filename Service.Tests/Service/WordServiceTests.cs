using Service.Repository;
using Data.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Humanizer;

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
            service = new WordService(this.uow.Object);
        }

        private Word dummy => new Word
        {
            ID = 13,
            SourceLanguageName = "polish",
            Properties = new()
            {
                new()
                {
                    ID = 5,
                    Name = "gender",
                    Values = new("masculine", "feminine")
                },

                new()
                {
                    ID = 6,
                    Name = "plural form",
                    Values = new("przeręble")
                },

                new()
                {
                    ID = 7,
                    Name = "genitive form",
                    Values = new("przerębla", "przerębli")
                }
            }
        };

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
        public void TryUpdate_WordFound_EverythingGodd_UpdatesProperly()
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

        // Attempt:
        // - Change only Properties
        // - Change Value
        // - Change both
        // - Change value to lower/uppercase
        // - Change properties to lower/uppercase
        // - Clear properties
        [Fact]
        public void TryUpdate_ChangePropertiesToDifferent_UpdatesProperly()
        {
            WordPropertySet props = new()
            {
                new()
                {
                    Name = "name1",
                    Values = new("value1", "value2")
                },

                new()
                {
                    Name = "name2",
                    Values = new("value1")
                }
            };

            var existing = dummy;
            Word @new = new()
            {
                ID = existing.ID,
                Value = existing.Value,
                Properties = props
            };
            //test merging languagename on living organism

            _wordRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == @new.ID))).Returns(true);
            //that should return 'existing', the word that is gonna be updated with '@new'
            _wordRepo.Setup(_ => _.GetByLanguageAndValue(@new.SourceLanguageName, @new.Value, false))
                .Returns(new Word[]{ existing });


            var result = service.TryUpdate(@new);

            _wordRepo.Verify(_ => _.Update(It.IsAny<Word>()), Times.Once);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void TryUpdate_ChangeValueToDifferent_UpdatesProperly()
        {
            var existing = dummy;
            Word @new = new()
            {
                ID = existing.ID,
                Properties = existing.Properties,
                Value = "a different value",
            };

            _wordRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == @new.ID))).Returns(true);
            //no other Words match this Word's Value because it was changed.
            _wordRepo.Setup(_ => _.GetByLanguageAndValue(@new.SourceLanguageName, @new.Value, false))
                .Returns(Enumerable.Empty<Word>());

            var result = service.TryUpdate(@new);

            _wordRepo.Verify(_ => _.Update(It.IsAny<Word>()), Times.Once);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void TryUpdate_ChangePropertiesAndValue_UpdatesProperly()
        {
            var existing = dummy;
            Word @new = new()
            {
                ID = existing.ID,
                Properties = new()
                {
                    new()
                    {
                        Name = "name",
                        Values = new("values")
                    }
                },
                Value = "new value",
            };

            _wordRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == @new.ID))).Returns(true);
            //no other Words match this Word's Value because it was changed.
            _wordRepo.Setup(_ => _.GetByLanguageAndValue(@new.SourceLanguageName, @new.Value, false))
                .Returns(Enumerable.Empty<Word>());

            var result = service.TryUpdate(@new);

            _wordRepo.Verify(_ => _.Update(It.IsAny<Word>()), Times.Once);
            Assert.True(result.IsValid);
        }

        [Fact]
        //so it returns the same word as the commited one
        public void TryUpdate_ChangeCaseOfValue_WordWithThatCaseAlreadyExists_ReturnsError()
        {
            var existing = dummy;
            existing.Value = "value";
            Word @new = new()
            {
                ID = existing.ID,
                Properties = existing.Properties,
                Value = "Value"
            };

            _wordRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == @new.ID))).Returns(true);
            _wordRepo.Setup(_ => _.GetByLanguageAndValue(@new.SourceLanguageName, @new.Value, false))
                .Returns(new Word[]{ @new });

            var result = service.TryUpdate(@new);

            _wordRepo.Verify(_ => _.Update(It.IsAny<Word>()), Times.Never);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void TryUpdate_ChangeCaseOfValue_UpdatesProperly()
        {
            //so basically the same as previously but the duplicate does not exist?
            var existing = dummy;
            existing.Value = "value";
            Word @new = new()
            {
                ID = existing.ID,
                Properties = existing.Properties,
                Value = "Value"
            };

            _wordRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == @new.ID))).Returns(true);
            _wordRepo.Setup(_ => _.GetByLanguageAndValue(@new.SourceLanguageName, @new.Value, false))
                .Returns(Array.Empty<Word>());

            var result = service.TryUpdate(@new);

            _wordRepo.Verify(_ => _.Update(It.IsAny<Word>()), Times.Once);
            Assert.True(result.IsValid);
        }

        //case of properties should be ignored
        [Fact]
        public void TryUpdate_ChangePropertiesCase_WordWithSuchPropertiesAlreadyExists_ReturnsError()
        {
            var existing = new Word
            {
                ID = 11,
                SourceLanguageName = "polish",
                Value = "some value",
                Properties = new()
                {
                    new()
                    {
                        Name = "name1",
                        Values = new("value1")
                    },

                    new()
                    {
                        Name = "name2",
                        Values = new("value1", "value2")
                    }
                }
            };

            var @new = new Word
            {
                ID = 11,
                SourceLanguageName = "polish",
                Value = "some value",
                Properties = new()
                {
                    new()
                    {
                        Name = "Name1",
                        Values = new("VALue1")
                    },

                    new()
                    {
                        Name = "NAme2",
                        Values = new("Value1", "value2")
                    }
                }
            };

            _wordRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == @new.ID))).Returns(true);
            _wordRepo.Setup(_ => _.GetByLanguageAndValue(@new.SourceLanguageName, @new.Value, false))
                .Returns(new Word[] { existing });

            var result = service.TryUpdate(@new);

            _wordRepo.Verify(_ => _.Update(It.IsAny<Word>()), Times.Never);
            Assert.False(result.IsValid);
        }
    }
}
