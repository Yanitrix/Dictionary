using Domain.Dto;
using Domain.Models;
using Domain.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Service.Tests.Service
{
    public class WordServiceTests : UowTestBase
    {
        IWordService service;
        Mock<IWordRepository> _wordRepo = new Mock<IWordRepository>();
        Mock<ILanguageRepository> _langRepo = new Mock<ILanguageRepository>();

        public WordServiceTests()
        {
            wordRepo = _wordRepo;
            langRepo = _langRepo;
            service = new WordService(this.uow.Object, this.mapper);
        }

        private CreateWord CreateDto() => new()
        {
            Value = "value",
            SourceLanguageName = "polish",
            Properties = new HashSet<WordPropertyDto>()
            {
                new()
                {
                    Name = "gender",
                    Values = new List<String>{ "masculine", "feminine" },
                },

                new()
                {
                    Name = "plural form",
                    Values = new List<String>{ "przeręble" }
                },

                new()
                {
                    Name = "genitive form",
                    Values = new List<String>{ "przerębla", "przerębli" }
                }
            }
        };

        private Word Existing() => new()
        {
            ID = 13,
            Value = "value",
            SourceLanguageName = "polish",
            Properties = new()
            {
                new()
                {
                    Name = "gender",
                    Values = new("masculine", "feminine"),
                },

                new()
                {
                    Name = "plural form",
                    Values = new("przeręble")
                },

                new()
                {
                    Name = "genitive form",
                    Values = new("przerębla", "przerębli")
                }
            }
        };

        private UpdateWord UpdateDto() => new()
        {
            ID = 13,
            Value = "value",
            Properties = new HashSet<WordPropertyDto>()
            {
                new()
                {
                    Name = "gender",
                    Values = new List<String>{ "masculine", "feminine" },
                },

                new()
                {
                    Name = "plural form",
                    Values = new List<String>{ "przeręble" }
                },

                new()
                {
                    Name = "genitive form",
                    Values = new List<String>{ "przerębla", "przerębli" }
                }
            }
        };

        private void Exists(bool exists = true)
        {
            _wordRepo.Setup(_ => _.ExistsByID(It.IsAny<int>())).Returns(exists);
        }

        private void LanguageExists(bool exists = true)
        {
            _langRepo.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(exists);
        }

        private void DuplicateIs(Word duplicate = null)
        {
            if (duplicate == null)
            {
                _wordRepo.Setup(_ => _.GetByLanguageAndValue(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<bool>()))
                    .Returns(Array.Empty<Word>());
                return;
            }

            _wordRepo.Setup(_ => _.GetByLanguageAndValue(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<bool>()))
                .Returns(new Word[] { duplicate });
        }

        private void ExistingIs(Word existing)
        {
            _wordRepo.Setup(_ => _.GetByID(It.IsAny<int>())).Returns(existing);
        }

        private void ShouldUpdate()
        {
            _wordRepo.Verify(_ => _.Update(It.IsAny<Word>()), Times.Once);
        }

        private void ShouldNotUpdate()
        {
            _wordRepo.Verify(_ => _.Update(It.IsAny<Word>()), Times.Never);
        }

        private void ShouldAdd()
        {
            _wordRepo.Verify(_ => _.Create(It.IsAny<Word>()), Times.Once);
        }

        private void ShouldNotAdd()
        {
            _wordRepo.Verify(_ => _.Create(It.IsAny<Word>()), Times.Never);
        }

        [Fact]
        public void TryAdd_LanguageExists_PropertiesGood_ShouldAdd()
        {
            LanguageExists(); //source language exists
            DuplicateIs(); //no duplicated words

            var dto = new CreateWord();

            var result = service.Add(dto);

            ShouldAdd();
            Assert.Empty(result);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void TryAdd_LanguageExists_AnotherWordWithSamePropertiesExist_ShouldReturnError()
        {
            var existing = new Word
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

            var dto = new CreateWord
            {
                Value = "Value", //uppercase
                Properties = new HashSet<WordPropertyDto>()
                {
                    new()
                    {
                        Name = "name1",
                        Values = new List<String> { "value1", "value2" }
                    },

                    new()
                    {
                        Name = "NAme2",
                        Values = new List<String> { "value3", "valuE4" }
                    },
                }
            };

            LanguageExists(); //source language exists
            DuplicateIs(existing);

            var result = service.Add(dto);

            ShouldNotAdd();
            Assert.Single(result);
            Assert.Equal("Duplicate", result.First().Name);
        }

        [Fact]
        public void TryAdd_PropertiesGood_LanguageDoesNotExist_ShouldReturnError()
        {
            DuplicateIs(); //no duplicated words
            LanguageExists(false); //source language doesnt exist

            var dto = CreateDto();
            var result = service.Add(dto);

            ShouldNotAdd();
            Assert.Single(result);
            Assert.Equal("Language does not exist.", result.First().Name);
        }

        [Fact]
        public void TryAdd_NorLanguageNorPropertiesAreGood_ShouldReturnError()
        {
            var dto = CreateDto();
            var duplicate = Existing();

            DuplicateIs(duplicate);
            LanguageExists(false); //source language doesnt exist

            var result = service.Add(dto);

            ShouldNotAdd();
            Assert.Single(result);
            Assert.Equal("Language does not exist.", result.First().Name);
        }

        [Fact]
        public void TryUpdate_WordDoesNotExist_ReturnsError()
        {
            var dto = new UpdateWord
            {
                ID = 1,
            };

            Exists(false);

            var result = service.Update(dto);

            ShouldNotUpdate();
            Assert.Single(result);
            Assert.Equal("Entity does not exist.", result.First().Name);
        }

        [Fact]
        public void TryUpdate_WordFound_EverythingGodd_UpdatesProperly()
        {
            var indb = Existing();
            var toUpdate = UpdateDto();
            toUpdate.Value = "changed value";

            Exists();
            ExistingIs(indb);
            LanguageExists();
            DuplicateIs();

            var result = service.Update(toUpdate);

            ShouldUpdate();
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
            Word existing = Existing();
            UpdateWord dto = UpdateDto();
            dto.Properties = new HashSet<WordPropertyDto>()
            {
                new()
                {
                    Name = "name1",
                    Values = new List<String>{"value1", "value2" }
                },

                new()
                {
                    Name = "name2",
                    Values = new List<String>{"value1" }
                }
            };

            Exists();
            ExistingIs(existing);
            DuplicateIs();

            var result = service.Update(dto);

            ShouldUpdate();
            Assert.True(result.IsValid);
        }

        [Fact]
        public void TryUpdate_ChangeValueToDifferent_UpdatesProperly()
        {
            UpdateWord dto = UpdateDto();
            Word existing = Existing();
            dto.Value = "a different value";

            Exists();
            //no other Words match this Word's Value because it was changed.
            ExistingIs(existing);
            DuplicateIs();

            var result = service.Update(dto);

            ShouldUpdate();
            Assert.True(result.IsValid);
        }

        [Fact]
        public void TryUpdate_ChangePropertiesAndValue_UpdatesProperly()
        {
            //no duplicate
            Word existing = Existing();
            UpdateWord dto = new()
            {
                ID = existing.ID,
                Value = "some other value than the previous one",
                Properties = new HashSet<WordPropertyDto>()
                {
                    new()
                    {
                        Name = "name",
                        Values = new List<String>{"value1", "value2"}
                    }
                }
            };

            Exists();
            ExistingIs(existing);
            DuplicateIs();

            var result = service.Update(dto);

            ShouldUpdate();
            Assert.True(result.IsValid);
        }

        [Fact]
        //so it returns the same word as the commited one
        public void TryUpdate_ChangeCaseOfValue_WordWithThatCaseAlreadyExists_ReturnsError()
        {
            var dto = UpdateDto();
            dto.Value = "Value";
            var existing = Existing();
            var duplicate = new Word
            {
                ID = 21,
                Value = dto.Value,
                Properties = existing.Properties,
                SourceLanguageName = existing.SourceLanguageName
            };

            Exists(true);
            ExistingIs(existing);
            DuplicateIs(duplicate);

            var result = service.Update(dto);

            ShouldNotUpdate();
            Assert.False(result.IsValid);
        }

        [Fact]
        public void TryUpdate_ChangeCaseOfValue_UpdatesProperly()
        {
            //so basically the same as previously but the duplicate does not exist?
            var dto = UpdateDto();
            dto.Value = "Value";
            Word existing = Existing();

            Exists();
            ExistingIs(existing);
            DuplicateIs();

            var result = service.Update(dto);

            ShouldUpdate();
            Assert.True(result.IsValid);
        }

        //case of properties should be ignored
    }
}
