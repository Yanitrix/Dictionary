using Data.Models;
using Data.Tests;
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

        private void reloadRepos()
        {
            this.changeContext();
            dictionaryRepo = new DictionaryRepository(this.context);
            repo = new EntryRepository(this.context);
        }

        private Dictionary[] createDictWithEverythingNeeded()
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
                LanguageInName = "english",
                LanguageOutName = "german",
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
                }
            };

            #endregion

            return new Dictionary[] { dict, dict1 };
        }

        //test both dictionaries
        [Fact]
        public void GetByID_ReturnsEntityWithLoadedReferences()
        {
            var dict = createDictWithEverythingNeeded()[0];

            dictionaryRepo.Create(dict);
            reloadRepos();

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
        public void GetByDictionaryAndWord_RetursProperEntityWithLoadedReferences()
        {
            var dict = createDictWithEverythingNeeded()[1];

            dictionaryRepo.Create(dict);
            reloadRepos();

            var dictIdx = dict.Index;
            var word = "stock";

            var expectedId = dict.Entries.First().ID;

            var foundCol = repo.GetByDictionaryAndWord(dictIdx, word);
            var found = foundCol.First();

            Assert.NotEmpty(foundCol);
            Assert.NotNull(found);
            Assert.Equal(expectedId, found.ID);
            Assert.Equal(word, found.Word.Value, true);
            Assert.Equal(dictIdx, found.DictionaryIndex);
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
            var dict = createDictWithEverythingNeeded()[1];

            dictionaryRepo.Create(dict);
            reloadRepos();

            var dictIdx = dict.Index;

            var foundCol = repo.GetByDictionaryAndWord(dictIdx, query);

            Assert.Empty(foundCol);
        }
    }
}
