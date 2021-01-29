using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Service.Repository;

namespace Service.Tests.Repositories
{
    public class EntryRepositoryTests : DbContextTestBase
    {
        IDictionaryRepository dictionaryRepo;
        IEntryRepository repo;

        public EntryRepositoryTests()
        {
            dictionaryRepo = new DictionaryRepository(this.context);
            repo = new EntryRepository(this.context);
        }

        private void Disconnect()
        {
            this.changeContext();
            dictionaryRepo = new DictionaryRepository(this.context);
            repo = new EntryRepository(this.context);
        }

        private Dictionary[] CreateDictionaryWithAllReferences()
        {
            //first has 2 entries
            #region english-german dictionary
            var dict = new Dictionary
            {
                LanguageIn = new Language
                {
                    Name = "english"
                },
                LanguageOut = new Language
                {
                    Name = "german"
                },
                Entries = new HashSet<Entry>
                {
                    new Entry
                    {
                        Word = new Word
                        {
                            Value = "stick",
                            SourceLanguageName = "english"
                        },

                        Meanings = new HashSet<Meaning>
                        {
                            new Meaning
                            {
                                Value = "Stock",
                                Examples = new HashSet<Example>
                                {
                                    new Example
                                    {
                                        Text = "to get the stick",
                                        Translation  = "den Stock bekommen"
                                    },

                                    new Example
                                    {
                                        Text = "to give sb the stick",
                                        Translation = "jdm eine Tracht Prügel verpassen"
                                    }
                                }
                            },

                            new Meaning
                            {
                                Value = "Zweig",
                                Notes = "small thin tree branch"
                            },

                            new Meaning
                            {
                                Value = "Zwangsmaßnahme (geeignetes Mittel, um etw. zu erreichen)",
                                Notes = "fig. means of coercion"
                            }
                        }
                    },

                    new Entry
                    {
                        Word = new Word
                        {
                            Value = "train",
                            SourceLanguageName = "english",
                            Properties = new WordPropertySet
                            {
                                new WordProperty
                                {
                                    Name = "speech part",
                                    Values = new StringSet{"noun"}
                                }
                            }
                        },

                        Meanings = new HashSet<Meaning>
                        {
                            new Meaning
                            {
                                Value = "Zug",
                                Examples = new HashSet<Example>
                                {
                                    new Example
                                    {
                                        Text = "to board a train",
                                        Translation = "in einen Zug einsteigen"
                                    }
                                }
                            },

                            new Meaning
                            {
                                Value = "Serie",
                                Notes = "series",
                                Examples = new HashSet<Example>
                                {
                                    new Example
                                    {
                                        Text = "to be in train",
                                        Translation = "im Gange sein"
                                    },

                                    new Example
                                    {
                                        Text = "a train of events",
                                        Translation = "eine Kette von Ereignissen"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            #endregion
            //second has 1 entry
            #region german-english dictionary

            var dict1 = new Dictionary
            {
                LanguageIn = new Language
                {
                    Name = "german"
                },
                LanguageOut = new Language
                {
                    Name = "english"
                },
                LanguageInName = "german",
                LanguageOutName = "english",
                Entries = new HashSet<Entry>
                {
                    new Entry
                    {
                        Word = new Word
                        {
                            SourceLanguageName = "german",
                            Value = "Stock",
                            Properties = new WordPropertySet
                            {
                                new WordProperty
                                {
                                    Name = "gender",
                                    Values = new StringSet{"masculine"}
                                }
                            }
                        },

                        Meanings = new HashSet<Meaning>
                        {
                            new Meaning
                            {
                                Value = "stick",
                                Notes = "lange Holzstange",
                            },

                            new Meaning
                            {
                                Value = "plant"
                            },

                            new Meaning
                            {
                                Value = "[bee]hive",
                                Notes = "Bienenstock"
                            },

                            new Meaning
                            {
                                Value = "floor",
                                Examples = new HashSet<Example>
                                {
                                    new Example
                                    {
                                        Text = "im 1. Stock",
                                        Translation = "on the first floor"
                                    }
                                }
                            },

                            new Meaning
                            {
                                Value = "storey",
                                Examples = new HashSet<Example>
                                {
                                    new Example
                                    {
                                        Text = "im 1. Stock",
                                        Translation = "on the first storey"
                                    }
                                }
                            }
                        },
                    },

                    new Entry
                    {
                        Word = new()
                        {
                            SourceLanguageName = "german",
                            Value = "stock"
                        }
                    }
                }
            };

            #endregion

            return new Dictionary[] { dict, dict1 };
        }

        private Dictionary CaseSensitiveDictionary()
        {
            return new Dictionary
            {
                LanguageIn = new() { Name = "in" },
                LanguageOut = new() { Name = "out" },
                Entries = new HashSet<Entry>()
                {
                    new()
                    {
                        Word = new()
                        {
                            SourceLanguageName = "in",
                            Value = "ąść"
                        }
                    },

                    new()
                    {
                        Word = new()
                        {
                            SourceLanguageName = "in",
                            Value = "Ąść"
                        }
                    },

                    new()
                    {
                        Word = new()
                        {
                            SourceLanguageName = "in",
                            Value = "ąŚĆ"
                        }
                    },

                    new()
                    {
                        Word = new()
                        {
                            SourceLanguageName = "in",
                            Value = "süßigkeiten"
                        }
                    },

                    new()
                    {
                        Word = new()
                        {
                            SourceLanguageName = "in",
                            Value = "Süßigkeiten"
                        }
                    },

                    new()
                    {
                        Word = new()
                        {
                            SourceLanguageName = "in",
                            Value = "SüßigkeITEN"
                        }
                    }
                }
            };
        }

        private Dictionary CaseInsensitiveDictionary()
        {
            return new Dictionary
            {
                LanguageIn = new() { Name = "in" },
                LanguageOut = new() { Name = "out" },
                Entries = new HashSet<Entry>()
                {
                    new()
                    {
                        Word = new()
                        {
                            SourceLanguageName = "in",
                            Value = "value"
                        }
                    },

                    new()
                    {
                        Word = new()
                        {
                            SourceLanguageName = "in",
                            Value = "Value"
                        }
                    },

                    new()
                    {
                        Word = new()
                        {
                            SourceLanguageName = "in",
                            Value = "Stick"
                        }
                    },

                    new()
                    {
                        Word = new()
                        {
                            SourceLanguageName = "in",
                            Value = "stick"
                        }
                    },

                    new()
                    {
                        Word = new()
                        {
                            SourceLanguageName = "in",
                            Value = "STIck"
                        }
                    },

                    new()
                    {
                        Word = new()
                        {
                            SourceLanguageName = "in",
                            Value = "SüßigkeITEN"
                        }
                    }
                }
            };
        }

        [Fact]
        public void GetByID_ReturnsEntityWithLoadedReferences()
        {
            var dict = CreateDictionaryWithAllReferences()[0];

            dictionaryRepo.Create(dict);
            Disconnect();

            var id = dict.Entries.First().ID;

            var found = repo.GetByID(id);

            Assert.NotNull(found);
            Assert.NotNull(found.Word);
            Assert.NotEmpty(found.Meanings);
            Assert.NotEmpty(found.Meanings.First().Examples);

            Assert.Equal(3, found.Meanings.Count);
            Assert.Equal(2, found.Meanings.First().Examples.Count);

            Assert.Equal("stick", found.Word.Value);
        }

        [Fact]
        public void GetByDictionaryAndWord_CaseSensitive_RetursProperEntityWithLoadedReferences()
        {
            var dict = CreateDictionaryWithAllReferences()[1];

            dictionaryRepo.Create(dict);
            Disconnect();

            var dictIdx = dict.Index;
            var word = "stock";

            var expectedId = dict.Entries.First().ID;

            var foundCollection = repo.GetByDictionaryAndWord(dictIdx, word);
            var found = foundCollection.First();

            Assert.Single(foundCollection);
            Assert.NotNull(found);
            Assert.Equal(word, found.Word.Value);
            Assert.Equal(dictIdx, found.DictionaryIndex);
        }

        [Fact]
        public void GetByDictionaryAndWord_CaseSensitive_CaseDoesNotMatch_ReturnsEmptyCollection()
        {
            var dict = CreateDictionaryWithAllReferences()[1];

            dictionaryRepo.Create(dict);
            Disconnect();

            var dictIdx = dict.Index;
            var word = "StocK";

            var found = repo.GetByDictionaryAndWord(dictIdx, word);

            Assert.Empty(found);
        }

        [Fact]
        public void GetByDictionaryAndWord_CaseInsensitive_ReturnsProperResults()
        {
            var dict = CreateDictionaryWithAllReferences()[1];

            dictionaryRepo.Create(dict);
            Disconnect();

            var dictIdx = dict.Index;
            var word = "StocK";

            var found = repo.GetByDictionaryAndWord(dictIdx, word, false);

            Assert.Equal(2, found.Count());
            foreach (var i in found)
                Assert.Equal(word, i.Word.Value, ignoreCase: true);
        }

        //edge cases: null, empty strings
        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData("\t\t")]
        [InlineData("\r\n")]
        [InlineData("\r")]
        [InlineData("\n\t")]
        public void GetByDictionaryAndWord_ArgsNullOrEmpty_ReturnsEmpty(String query)
        {
            var dict = CreateDictionaryWithAllReferences()[1];

            dictionaryRepo.Create(dict);
            Disconnect();

            var dictIdx = dict.Index;

            var foundCol = repo.GetByDictionaryAndWord(dictIdx, query);

            Assert.Empty(foundCol);
        }

        [Theory]
        [InlineData("ąść")]
        [InlineData("Ąść")]
        [InlineData("Süßigkeiten")]
        public void GetByWord_CaseSensitive_ReturnsProperEntries(String word)
        {
            var dictionary = CaseSensitiveDictionary();

            context.Dictionaries.Add(dictionary);
            context.SaveChanges();
            Disconnect();

            var found = repo.GetByWord(word);
            var entry = found.First();

            Assert.Single(found);
            Assert.Equal(word, entry.Word.Value, ignoreCase: false);

        }

        [Theory]
        [InlineData("ascascasc")]
        [InlineData("ĄŚĆ")]
        [InlineData("SÜßigkeiten")]
        [InlineData("bbbasd")]
        [InlineData("word")]
        [InlineData("\t\t")]
        [InlineData("\t\n")]
        [InlineData("\r")]
        [InlineData(" ")]
        [InlineData("")]
        public void GetByWord_CaseSensitive_EntryDoesNotExistOrCaseDoesNotMatch_ReturnsEmptyCollection(String word)
        {
            var dictionary = CaseSensitiveDictionary();

            context.Dictionaries.Add(dictionary);
            context.SaveChanges();
            Disconnect();

            var found = repo.GetByWord(word);
            Assert.Empty(found);
        }

        [Theory]
        [InlineData("stick", 3)]
        [InlineData("value", 2)]
        public void GetByWord_CaseInsensitive_ReturnsProperEntries(String word, int amount)
        {
            var dictionary = CaseInsensitiveDictionary();

            context.Dictionaries.Add(dictionary);
            context.SaveChanges();
            Disconnect();

            var found = repo.GetByWord(word, false);

            Assert.Equal(amount, found.Count());
            foreach (var i in found)
                Assert.Equal(word, i.Word.Value, ignoreCase: true);
        }

        [Fact]
        public void HasMeanings_EntryHasMeanings_ReturnsTrue()
        {
            var dictionary = new Dictionary
            {
                LanguageIn = new() { Name = "in" },
                LanguageOut = new() { Name = "out" },
            };

            var entry = new Entry
            {
                Word = new()
                {
                    SourceLanguageName = "in",
                    Value = "value"
                },
                Meanings = new HashSet<Meaning>()
                {
                    new()
                    {
                        Value = "val1"
                    },

                    new()
                    {
                        Value = "val2"
                    },

                    new()
                    {
                        Value = "val3"
                    },
                }
            };

            dictionary.Entries.Add(entry);
            context.Dictionaries.Add(dictionary);
            context.SaveChanges();
            var id = entry.ID;
            Disconnect();
            
            //act
            var result = repo.HasMeanings(id);

            //assert
            Assert.True(result);
        }

        [Fact]
        public void HasMeanings_EntryDoesNot_ReturnsFalse()
        {
            var dictionary = new Dictionary
            {
                LanguageIn = new() { Name = "in" },
                LanguageOut = new() { Name = "out" },
            };

            var entry = new Entry
            {
                Word = new()
                {
                    SourceLanguageName = "in",
                    Value = "value"
                },
            };

            dictionary.Entries.Add(entry);
            context.Dictionaries.Add(dictionary);
            context.SaveChanges();
            var id = entry.ID;
            Disconnect();

            //act
            var result = repo.HasMeanings(id);

            //assert
            Assert.False(result);
        }

        //actually tests if has meanings
        //getbyword CS and CI
    }
}
