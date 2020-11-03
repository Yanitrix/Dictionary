﻿using Data.Models;
using Data.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Service.Repository;

namespace Service.Tests.Repositories
{
    public class WordRepositoryTests : DbContextTestBase
    {
        private WordRepository repository;

        public WordRepositoryTests()
        {
            repository = new WordRepository(this.context);

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
                        Values = new StringSet{"masculine"}
                    },

                    new WordProperty
                    {
                        Name = "plural form",
                        Values = new StringSet{"indexes", "indices"}
                    }
                }
            };

            repository.Create(word);

            var idInDb = word.ID;

            var wordInDb = repository.All().First();

            Assert.Equal(2, wordInDb.Properties.Count);
            Assert.Equal(idInDb, wordInDb.ID);

        }

        [Fact]
        public void UpdateWord_FieldsShouldChange()
        {
            var entity = new Word
            {
                SourceLanguage = new Language
                {
                    Name = "hs"
                },
                SourceLanguageName = "hs",
                Value = "hs",
                Properties = new WordPropertySet
                {
                    new WordProperty
                    {
                        Name = "speech part",
                        Values = new StringSet{"noun"}
                    }
                }
            };

            String newLanguageName = "russian";
            String newValue = "a normal word";

            repository.Create(entity);
            var entityInDb = repository.All().First();
            entityInDb.SourceLanguageName = "russian";
            entityInDb.SourceLanguage = new Language { Name = newLanguageName };
            entityInDb.Value = "a normal word";

            repository.Update(entityInDb);
            var updated = repository.All().First();

            Assert.Equal(newValue, updated.Value);
            Assert.Equal(newLanguageName, updated.SourceLanguageName);

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


        [Theory]
        [InlineData(4)]
        public void GetByID_NotFound_ShouldReturnNull(int givenId)
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

            var found = repository.GetByID(givenId);

            Assert.NotEqual(entity.ID, givenId);
            Assert.Null(found);

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

            repository.CreateRange(entity1, entity2);

            var first = repository.GetByID(1);
            var second = repository.GetByID(2);

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
        public void GetByValue_ShouldNotBeEmpty_ShouldBeOrderedProperly()
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
                    Value = "stick"
                },

                new Word
                {
                    SourceLanguageName = "arabic",
                    Value = "not-a-stick"
                },

                new Word
                {
                    SourceLanguage = new Language
                    {
                        Name = "not so arabic"
                    },
                    SourceLanguageName = "not so arabic",
                    Value = "stick"
                },

                new Word
                {
                    SourceLanguageName = "not so arabic",
                    Value = "not-a-stick"
                },

                new Word
                {
                    SourceLanguage = new Language
                    {
                        Name = "zimbabwean"
                    },
                    SourceLanguageName = "zimbabwean",
                    Value = "stick"
                },
            };

            repository.CreateRange(entities);

            var foundWords = repository.GetByValue("stick");
            var indexed = new List<Word>(foundWords);

            Assert.Equal(3, foundWords.Count());

            Assert.Equal("arabic", indexed[0].SourceLanguageName);
            Assert.Equal("stick", indexed[0].Value);

            Assert.Equal("not so arabic", indexed[1].SourceLanguageName);
            Assert.Equal("stick", indexed[1].Value);

            Assert.Equal("zimbabwean", indexed[2].SourceLanguageName);
            Assert.Equal("stick", indexed[2].Value);

        }

        [Fact]
        public void GetByValue_NotFound_ShouldBeEmpty()
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
                    Value = "stick"
                },

                new Word
                {
                    SourceLanguage = new Language
                    {
                        Name = "not so arabic"
                    },
                    SourceLanguageName = "not so arabic",
                    Value = "not-a-stick"
                },

                new Word
                {
                    SourceLanguage = new Language
                    {
                        Name = "zimbabwean"
                    },
                    SourceLanguageName = "zimbabwean",
                    Value = "stick"
                },
            };

            repository.CreateRange(entities);

            var found = repository.GetByValue("that's not a stick");

            Assert.Empty(found);

        }

        //TODO querying strings not working
        [Fact]
        public void GetByLanguageAndValue_ShouldNotBeEmpty()
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
                    Value = "stick"
                },

                new Word
                {
                    SourceLanguageName = "arabic",
                    Value = "not-a-stick"
                },

                new Word
                {
                    SourceLanguageName = "arabic",
                    Value = "stick"
                },

                new Word
                {
                    SourceLanguage = new Language
                    {
                        Name = "not so arabic"
                    },
                    SourceLanguageName = "not so arabic",
                    Value = "stick"
                },

                new Word
                {
                    SourceLanguageName = "not so arabic",
                    Value = "not-a-stick"
                },

                new Word
                {
                    SourceLanguage = new Language
                    {
                        Name = "zimbabwean"
                    },
                    SourceLanguageName = "zimbabwean",
                    Value = "stick"
                },
            };

            repository.CreateRange(entities);

            var found = repository.GetByLanguageAndValue("arabic", "stick");

            Assert.Equal(2, found.Count());

            foreach (var i in found)
            {
                Assert.Equal("arabic", i.SourceLanguageName);
                Assert.Equal("stick", i.Value);
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
                            Values = new StringSet{"noun", "verb" }
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
                            Values = new StringSet{"noun", "verb" }
                        },

                        new WordProperty
                        {
                            Name = "gender",
                            Values = new StringSet{"masculine"}
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
                            Values = new StringSet{"noun", "verb" }
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
                            Values = new StringSet{"noun", "verb" }
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
                            Values = new StringSet{"noun", "verb" }
                        },

                        new WordProperty
                        {
                            Name = "gender",
                            Values = new StringSet{"masculine"}
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
                            Values = new StringSet{"noun", "verb" }
                        }
                    }
                },

            };

            repository.Create(entities[0]);
            repository.Create(entities[1]);
            repository.Create(entities[2]);

            this.changeContext();
            repository = new WordRepository(this.context);

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
