using Domain.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Domain.Repository;
using System;
using Persistance.Repository;

namespace Persistence.Tests.Repositories
{
    public class WordRepositoryTests : DbContextTestBase
    {
        private IWordRepository repository;

        public WordRepositoryTests()
        {
            repository = new WordRepository(this.context);
        }

        private void Disconnect()
        {
            changeContext();
            repository = new WordRepository(context);
        }

        private void CreateLanguages()
        {
            var arabic = new Language
            {
                Name = "arabic"
            };

            var notArabic = new Language
            {
                Name = "not so arabic"
            };

            var zimbabwean = new Language
            {
                Name = "zimbabwean"
            };

            context.Languages.AddRange(arabic, notArabic, zimbabwean);
        }

        [Fact]
        public void AddWordWithProperties_ShouldExist()
        {
            Word word = new Word
            {
                SourceLanguage = new Language
                {
                    Name = "russian"
                },
                SourceLanguageName = "russian",
                Value = "index",
                Properties = new WordPropertySet
                {
                    new WordProperty
                    {
                        Name = "gender",
                        Values = new("masculine")
                    },

                    new WordProperty
                    {
                        Name = "plural form",
                        Values = new("indexes", "indices")
                    }
                }
            };

            repository.Create(word);
            Disconnect();

            var idInDb = word.ID;

            var wordInDb = repository.All().First();
            //im not gonna check if properties are loaded because there is no reason to get all Words in repo and include their properties.
            Assert.Equal(idInDb, wordInDb.ID);
        }

        [Fact]
        public void UpdateWord_ValueAndPropertiesChange_LanguageDoesNot()
        {
            //arrange
            var english = new Language { Name = "english" };
            var german = new Language { Name = "german" };
            context.Languages.AddRange(english, german);

            var word = new Word
            {
                SourceLanguageName = "english",
                Properties = new()
                {
                    new()
                    {
                        Name = "name1",
                        Values = new("value1", "value2"),
                    },

                    new()
                    {
                        Name = "name2",
                        Values = new("value1", "value2")
                    }
                },
                Value = "value"
            };

            context.Words.Add(word);
            context.SaveChanges();

            Disconnect();
            //act
            word.Value = "some other value";
            word.Properties = new()
            {
                new()
                {
                    Name = "name3",
                    Values = new("value1", "value2", "value3")
                }
            };
            word.SourceLanguageName = "german";

            repository.Update(word);
            var inDB = repository.GetByID(word.ID);

            //assert
            Assert.Equal("some other value", inDB.Value);
            Assert.Single(inDB.Properties);
            Assert.Equal("name3", inDB.Properties.First().Name);
            Assert.Equal(3, inDB.Properties.First().Values.Count);
            Assert.NotEqual("german", inDB.SourceLanguageName);
            Assert.Equal("english", inDB.SourceLanguageName);
        }

        [Fact]
        public void DeleteWordWithoutProperties_ShouldNotExist()
        {
            var entity = new Word
            {
                SourceLanguage = new Language
                {
                    Name = "hs"
                },
                SourceLanguageName = "hs",
                Value = "stick"
            };

            repository.Create(entity);
            repository.Delete(entity);

            Assert.False(repository.All().Any());
        }

        [Fact]
        public void GetByID_Exists_ShouldReturnValue()
        {
            var entity1 = new Word
            {
                SourceLanguage = new Language
                {
                    Name = "sk"
                },
                SourceLanguageName = "sk",
                Value = "pot"
            };

            var entity2 = new Word
            {
                SourceLanguage = new Language
                {
                    Name = "ów"
                },
                SourceLanguageName = "ów",
                Value = "hot"
            };

            context.Words.AddRange(entity1, entity2);
            context.SaveChanges();
            int firstId = entity1.ID, secondId = entity2.ID;
            Disconnect();

            var first = repository.GetByID(firstId);
            var second = repository.GetByID(secondId);

            Assert.NotNull(first);
            Assert.Equal(1, first.ID);
            Assert.Equal("sk", first.SourceLanguageName);
            Assert.Equal("pot", first.Value);

            Assert.NotNull(second);
            Assert.Equal(2, second.ID);
            Assert.Equal("ów", second.SourceLanguageName);
            Assert.Equal("hot", second.Value);
        }

        [Fact]
        public void GetByValue_ShouldBeCaseSensitive_ShouldBeOrderedProperly()
        {
            Word[] entities =
            {
                new Word
                {
                    SourceLanguageName = "arabic",
                    Value = "ąść"
                },

                new Word
                {
                    SourceLanguageName = "not so arabic",
                    Value = "Ąść"
                },

                new Word
                {
                    SourceLanguageName = "arabic",
                    Value = "not-a-ąść"
                },

                new Word
                {
                    SourceLanguageName = "not so arabic",
                    Value = "ąść"
                },

                new Word
                {
                    SourceLanguageName = "not so arabic",
                    Value = "not-a-stick"
                },

                new Word
                {
                    SourceLanguageName = "zimbabwean",
                    Value = "ąść"
                },
            };

            CreateLanguages();
            context.Words.AddRange(entities);
            context.SaveChanges();
            Disconnect();

            //act
            var expected = "ąść";
            var foundWords = repository.GetByValue(expected);
            var indexed = new List<Word>(foundWords);

            Assert.Equal(3, foundWords.Count());
            
            Assert.Equal("arabic", indexed[0].SourceLanguageName);
            Assert.Equal(expected, indexed[0].Value);

            Assert.Equal("not so arabic", indexed[1].SourceLanguageName);
            Assert.Equal(expected, indexed[1].Value);

            Assert.Equal("zimbabwean", indexed[2].SourceLanguageName);
            Assert.Equal(expected, indexed[2].Value);
        }

        [Theory]
        [InlineData("chrzan")]
        [InlineData("stöckE")]
        [InlineData("HSTUS")]
        public void GetByValue_ShouldBeCaseInsensitive_Found(String word)
        {
            Word[] entities =
            {
                new()
                {
                    SourceLanguageName = "arabic",
                    Value = "Chrzan"
                },

                new()
                {
                    SourceLanguageName = "arabic",
                    Value = "CHrzaN"
                },

                new()
                {
                    SourceLanguageName = "not so arabic",
                    Value = "Stöcke"
                },

                new()
                {
                    SourceLanguageName = "not so arabic",
                    Value = "sTöcke"
                },

                new()
                {
                    SourceLanguageName = "zimbabwean",
                    Value = "hstus"
                },

                new()
                {
                    SourceLanguageName = "zimbabwean",
                    Value = "HSTus"
                },
            };

            CreateLanguages();
            context.Words.AddRange(entities);
            context.SaveChanges();

            var found = repository.GetByValue(word, false);

            Assert.NotEmpty(found);
            Assert.Equal(2, found.Count());
            foreach(var i in found)
            {
                Assert.Equal(word, i.Value, ignoreCase: true);
            }
        }

        [Fact]
        public void GetByValue_NotFound_ShouldBeEmpty()
        {
            Word[] entities =
            {
                new Word
                {
                    SourceLanguageName = "arabic",
                    Value = "stick"
                },

                new Word
                {
                    SourceLanguageName = "not so arabic",
                    Value = "not-a-stick"
                },

                new Word
                {
                    SourceLanguageName = "zimbabwean",
                    Value = "stick"
                },
            };

            CreateLanguages();
            context.Words.AddRange(entities);
            Disconnect();

            var found = repository.GetByValue("sti");

            Assert.Empty(found);
        }
        [Fact]
        public void GetByLanguageAndValue_ShouldBeCaseSensitive_WordExists()
        {
            Word[] entities =
            {
                new()
                {
                    Value = "stick",
                    SourceLanguageName = "arabic"
                },

                new()
                {
                    Value = "Stick",
                    SourceLanguageName = "arabic"
                },

                new()
                {
                    Value = "nor stick nor Stick",
                    SourceLanguageName = "arabic"
                },

                new()
                {
                    Value = "stick",
                    SourceLanguageName = "not so arabic"
                },

                new()
                {
                    Value = "Stick",
                    SourceLanguageName = "not so arabic"
                },

                new()
                {
                    Value = "not a stick",
                    SourceLanguageName = "zimbabwean"
                }
            };

            CreateLanguages();
            context.Words.AddRange(entities);
            context.SaveChanges();
            Disconnect();

            //act
            //case sensitive call
            var found = repository.GetByLanguageAndValue("arabic", "stick");

            Assert.Single(found);

            foreach (var i in found)
            {
                Assert.Equal("arabic", i.SourceLanguageName);
                Assert.Equal("stick", i.Value);
            }
        }

        [Theory]
        [InlineData("Arabic")] //case does not match with "arabic"
        [InlineData("nonexistent")] //does not exist
        [InlineData("\n\n")] //edge cases
        [InlineData("\n\r")]
        [InlineData("\t")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetByLanguageAndValue_ShouldBeCaseSensitive_LanguageDoesNotExistOrCaseDoesNotMatch_ReturnsEmpty(String language)
        {
            Word[] entities =
            {
                new()
                {
                    Value = "betrügen",
                    SourceLanguageName = "arabic"
                },

                new()
                {
                    Value = "betrügen",
                    SourceLanguageName = "not so arabic"
                },

                new()
                {
                    Value = "Betrügen",
                    SourceLanguageName = "not so arabic"
                },

                new()
                {
                    Value = "nicht betrügen",
                    SourceLanguageName = "zimbabwean"
                }
            };

            CreateLanguages();
            context.Words.AddRange(entities);
            context.SaveChanges();
            Disconnect();

            //act
            var found = repository.GetByLanguageAndValue(language, "betrügen");
            
            //assert
            Assert.Empty(found);
        }

        [Fact]
        public void GetByLanguageAndValue_ShouldBeCaseInsensitive()
        {
            Word[] entities =
            {
                new()
                {
                    Value = "stick",
                    SourceLanguageName = "arabic"
                },

                new()
                {
                    Value = "Stick",
                    SourceLanguageName = "arabic"
                },

                new()
                {
                    Value = "nor stick nor Stick",
                    SourceLanguageName = "arabic"
                },

                new()
                {
                    Value = "stick",
                    SourceLanguageName = "not so arabic"
                },

                new()
                {
                    Value = "Stick",
                    SourceLanguageName = "not so arabic"
                },

                new()
                {
                    Value = "not a stick",
                    SourceLanguageName = "zimbabwean"
                }
            };

            CreateLanguages();
            context.Words.AddRange(entities);
            context.SaveChanges();
            Disconnect();

            //act
            //case insensitive call
            var found = repository.GetByLanguageAndValue("arabic", "stick", false);

            Assert.Equal(2, found.Count());
            foreach (var i in found)
            {
                Assert.Equal("arabic", i.SourceLanguageName);
                Assert.Equal("stick", i.Value, ignoreCase: true);
            }
        }

        [Fact]
        public void GetOne_ShouldNotReturnNull_PropertiesLoaded()
        {
            Word[] entities =
            {
                new Word
                {
                    SourceLanguage = new Language
                    {
                        Name = "arabic"
                    },
                    SourceLanguageName = "arabic",
                    Value = "stick",
                    Properties = new WordPropertySet
                    {
                        new WordProperty
                        {
                            Name = "speech part",
                            Values = new("noun", "verb")
                        }
                    }
                },

                new Word
                {
                    SourceLanguageName = "arabic",
                    Value = "sticky",
                    Properties = new WordPropertySet
                    {
                        new WordProperty
                        {
                            Name = "speech part",
                            Values = new("noun", "verb")
                        },

                        new WordProperty
                        {
                            Name = "gender",
                            Values = new("masculine")
                        }
                    }
                },

                new Word
                {
                    SourceLanguageName = "arabic",
                    Value = "not so sticky",
                    Properties = new WordPropertySet
                    {
                        new WordProperty
                        {
                            Name = "speech part",
                            Values = new("noun", "verb")
                        }
                    }
                },
            };

            repository.Create(entities[0]);
            repository.Create(entities[1]);
            repository.Create(entities[2]);

            //changing context so that entities must be loaded from db, not from memory
            this.changeContext();
            repository = new WordRepository(this.context);

            var found = repository.GetOne(w => w.Value == "sticky");

            Assert.NotNull(found);
            Assert.NotEmpty(found.Properties);
            Assert.Equal(2, found.Properties.Count);
        }

        [Fact]
        public void Get_ShouldNotBeEmpty_PropertiesLoaded()
        {
            Word[] entities =
            {
                new Word
                {
                    SourceLanguageName = "arabic",
                    Value = "stick",
                    Properties = new WordPropertySet
                    {
                        new WordProperty
                        {
                            Name = "speech part",
                            Values = new("noun", "verb")
                        }
                    }
                },

                new Word
                {
                    SourceLanguageName = "arabic",
                    Value = "sticky",
                    Properties = new WordPropertySet
                    {
                        new WordProperty
                        {
                            Name = "speech part",
                            Values = new("noun", "verb")
                        },

                        new WordProperty
                        {
                            Name = "gender",
                            Values = new("masculine")
                        }
                    }
                },

                new Word
                {
                    SourceLanguageName = "arabic",
                    Value = "not so sticky",
                    Properties = new WordPropertySet
                    {
                        new WordProperty
                        {
                            Name = "speech part",
                            Values = new("noun", "verb")
                        }
                    }
                },
            };

            CreateLanguages();
            repository.Create(entities[0]);
            repository.Create(entities[1]);
            repository.Create(entities[2]);

            Disconnect();

            var foundWords = repository.Get(w => w.Value.StartsWith("stick"), x => x);
            var indexed = new List<Word>(foundWords);

            Assert.Equal(2, indexed.Count);

            Assert.Equal("arabic", indexed[0].SourceLanguageName);
            Assert.Equal("stick", indexed[0].Value);
            Assert.Single(indexed[0].Properties);

            Assert.Equal("arabic", indexed[1].SourceLanguageName);
            Assert.Equal("sticky", indexed[1].Value);
            Assert.Equal(2, indexed[1].Properties.Count);

        }
    }
}
