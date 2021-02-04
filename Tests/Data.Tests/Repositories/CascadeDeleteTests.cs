using Data.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Data.Repository;

namespace Data.Tests.Repositories
{
    public class CascadeDeleteTests : DbContextTestBase
    {
        ILanguageRepository langRepo;
        IWordRepository wordRepo;
        IDictionaryRepository dictRepo;
        IEntryRepository entryRepo;
        IMeaningRepository meaningRepo;
        IFreeExpressionRepository freeExpressionRepo;
        IExampleRepository exampleRepo;

        public CascadeDeleteTests()
        {
            langRepo = new LanguageRepository(this.context);
            wordRepo = new WordRepository(this.context);
            dictRepo = new DictionaryRepository(this.context);
            entryRepo = new EntryRepository(this.context);
            meaningRepo = new MeaningRepository(this.context);
            freeExpressionRepo = new FreeExpressionRepository(this.context);
            exampleRepo = new ExampleRepository(this.context);
        }

        private void reloadRepos()
        {
            changeContext();
            langRepo = new LanguageRepository(this.context);
            wordRepo = new WordRepository(this.context);
            dictRepo = new DictionaryRepository(this.context);
            entryRepo = new EntryRepository(this.context);
            meaningRepo = new MeaningRepository(this.context);
            freeExpressionRepo = new FreeExpressionRepository(this.context);
            exampleRepo = new ExampleRepository(this.context);
        }

        /*
         to delete:
        dictionary with free expressions, //done
        meaning with examples,  //done

        entry with meanings, //done
        dictionary with entries, //done
        language with words //done

        word with entries, //done
        language with dictionaries //done
        */

        private Word dummyWord
        {
            get => new Word
            {
                SourceLanguageName = "in",
                Value = "value"
            };
        }

        [Fact]
        public void DeleteDictionaryWithExpressions()
        {
            var dict = new Dictionary
            {
                LanguageIn = new Language { Name = "in" },
                LanguageOut = new Language { Name = "out" },
                FreeExpressions = new List<FreeExpression>
                {
                    new FreeExpression
                    {
                        Text = "text",
                        Translation = "translation"
                    },

                    new FreeExpression
                    {
                        Text = "text",
                        Translation = "Transllation"
                    },

                    new FreeExpression
                    {
                        Text = "text",
                        Translation = "translation"
                    },

                    new FreeExpression
                    {
                        Text = "text",
                        Translation = "translation"
                    },
                }
            };

            var exp = new FreeExpression
            {
                Dictionary = new Dictionary
                {
                    LanguageInName = "out",
                    LanguageOutName = "in"
                },

                Text = "sdasda",
                Translation = "sdasd"
            };

            dictRepo.Create(dict);
            freeExpressionRepo.Create(exp);

            Assert.NotEmpty(dictRepo.All());
            Assert.Equal(5, freeExpressionRepo.All().Count());

            reloadRepos();
            dictRepo.Delete(dict);

            Assert.Single(dictRepo.All());
            Assert.Single(freeExpressionRepo.All());
        }

        [Fact]
        public void DeleteMeaningWithExpressions()
        {
            var meaning = new Meaning
            {
                Entry = new Entry
                {
                    Dictionary = new Dictionary
                    {
                        LanguageIn = new Language { Name = "in" },
                        LanguageOut = new Language { Name = "out" },
                    },

                    Word = dummyWord,
                },

                Examples = new List<Example>
                {
                    new Example
                    {
                        Text = "text",
                        Translation = "translation"
                    },

                    new Example
                    {
                        Text = "text",
                        Translation = "Transllation"
                    },

                    new Example
                    {
                        Text = "text",
                        Translation = "translation"
                    },
                }
            };

            var dict = new Dictionary
            {
                LanguageInName = "out",
                LanguageOutName = "in",
                Entries = new HashSet<Entry>
                {
                    new Entry
                    {
                        Word = new Word { SourceLanguageName = "out", Value = "v"},
                        Meanings = new HashSet<Meaning>
                        {
                            new Meaning
                            {
                                Value = "s",
                                Examples = new HashSet<Example>
                                {
                                    new Example
                                    {
                                        Text = "a",
                                        Translation = "b"
                                    },

                                    new Example
                                    {
                                        Text = "a",
                                        Translation = "b"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            meaningRepo.Create(meaning);
            dictRepo.Create(dict);

            Assert.NotEmpty(meaningRepo.All());
            Assert.Equal(5, exampleRepo.All().Count());

            reloadRepos();
            meaningRepo.Delete(meaning);

            Assert.Single(meaningRepo.All());
            Assert.Equal(2, exampleRepo.All().Count());
        }

        [Fact]
        public void DeleteEntryWithMeanings()
        {
            var dict = new Dictionary
            {
                LanguageIn = new Language { Name = "in" },
                LanguageOut = new Language { Name = "out" },
            };

            Meaning[] meanings =
            {
                new Meaning
                {
                    Value = "free1"
                },

                new Meaning
                {
                    Value = "free2"
                }
            };

            Entry[] entries =
            {
                new Entry
                {
                    Word = dummyWord,

                    Meanings = new HashSet<Meaning>
                    {
                        new Meaning
                        {
                            Value = "entry1.meaning1"
                        },

                        new Meaning
                        {
                            Value = "entry1.meaning2"
                        }
                    }
                },

                new Entry
                {
                    Word = dummyWord,

                    Meanings = new HashSet<Meaning>
                    {
                        new Meaning
                        {
                            Value = "entry2.meaning1"
                        },

                        new Meaning
                        {
                            Value = "entry2.meaning2"
                        },
                        new Meaning
                        {
                            Value = "entry2.meaning3"
                        },
                    }
                },

                new Entry
                {
                    Word = dummyWord,

                    Meanings = meanings.ToHashSet(),
                }
            };

            dict.Entries = entries.ToHashSet();

            dictRepo.Create(dict);

            Assert.Equal(3, entryRepo.All().Count());
            Assert.Equal(7, meaningRepo.All().Count());

            entryRepo.Delete(entries[0]);

            Assert.Equal(2, entryRepo.All().Count());
            Assert.Equal(5, meaningRepo.All().Count());
        }

        [Fact]
        public void DeleteDictionaryWithEntries()
        {
            var dict = new Dictionary
            {
                LanguageIn = new Language { Name = "in" },
                LanguageOut = new Language { Name = "out" },
                Entries = new HashSet<Entry>
                {
                    new Entry
                    {
                        Word = dummyWord,
                    },

                    new Entry
                    {
                        Word = dummyWord,
                    }
                }
            };

            var dict1 = new Dictionary
            {
                LanguageInName = "out",
                LanguageOutName = "in",
                Entries = new HashSet<Entry>
                {
                    new Entry
                    {
                        Word = dummyWord,
                    },

                    new Entry
                    {
                        Word = dummyWord,
                    },

                    new Entry
                    {
                        Word = dummyWord,
                    },

                    new Entry
                    {
                        Word = dummyWord,
                    }
}
            };

            Entry[] entries =
            {
                new Entry
                {
                    Dictionary = dict1,
                    Word = dummyWord,
                }
            };

            dictRepo.Create(dict);
            dictRepo.Create(dict1);
            entryRepo.CreateRange(entries);

            Assert.Equal(2, dictRepo.All().Count());
            Assert.Equal(7, entryRepo.All().Count());

            dictRepo.Delete(dict1);

            Assert.Single(dictRepo.All());
            Assert.Equal(2, entryRepo.All().Count());
        }

        [Fact]
        public void DeleteLanguageWithWords()
        {
            var lang1 = new Language
            {
                Name = "polish",
                Words = new HashSet<Word>
                {
                    new Word
                    {
                        Value = "patyk"
                    },

                    new Word
                    {
                        Value = "spać"
                    },

                    new Word
                    {
                        Value = "jeść"
                    }
                }
            };

            var lang2 = new Language
            {
                Name = "german",
                Words = new HashSet<Word>
                {
                    new Word
                    {
                        Value = "Stock"
                    },

                    new Word
                    {
                        Value = "schlafen"
                    },
                }
            };

            Word[] words =
            {
                new Word
                {
                    SourceLanguage = new Language { Name = "russian" },
                    Value = " skdjaksjdasd"
                },

                new Word
                {
                    SourceLanguage = new Language { Name = "english" },
                    Value = "sadsdas"
                }
            };

            langRepo.Create(lang1);
            langRepo.Create(lang2);
            wordRepo.CreateRange(words);

            Assert.Equal(4, langRepo.All().Count());
            Assert.Equal(7, wordRepo.All().Count());

            langRepo.Delete(lang1);

            Assert.Equal(3, langRepo.All().Count());
            Assert.Equal(4, wordRepo.All().Count());

            langRepo.Delete(lang2);

            Assert.NotEmpty(langRepo.All());
            Assert.Equal(2, langRepo.All().Count());
            Assert.Equal(2, wordRepo.All().Count());
        }

        [Fact]
        public void DeleteWordWithEntries()
        {
            var dict = new Dictionary
            {
                LanguageIn = new Language { Name = "english"},
                LanguageOut = new Language { Name = "german"},
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

            dictRepo.Create(dict);

            Assert.Equal(2, wordRepo.All().Count());
            Assert.Equal(2, entryRepo.All().Count());

            var stick = wordRepo.GetByLanguageAndValue("English", "Stick").FirstOrDefault();

            wordRepo.Delete(stick);

            Assert.Single(dictRepo.All());
            Assert.Single(wordRepo.All());
            Assert.Single(entryRepo.All());
        }

        [Fact]
        public void DeleteLanguageWithDictionaries()
        {
            var english = new Language { Name = "english" };
            var polish = new Language { Name = "polish" };
            var german = new Language { Name = "german" };
            var russian = new Language { Name = "russian" };
            var langs = new Language[] { english, polish, german, russian };

            Dictionary[] entities =
            {
                new Dictionary
                {
                    LanguageInName = "english",
                    LanguageOutName = "german"
                },

                new Dictionary
                {
                    LanguageInName = "polish",
                    LanguageOutName = "german"
                },

                new Dictionary
                {
                    LanguageInName = "russian",
                    LanguageOutName = "polish"
                },

                new Dictionary
                {
                    LanguageInName = "english",
                    LanguageOutName = "russian"
                }
            };

            langRepo.CreateRange(langs);
            dictRepo.CreateRange(entities);

            Assert.Equal(4, langRepo.All().Count());
            Assert.Equal(4, dictRepo.All().Count());

            dictRepo.Delete(entities[2]);

            Assert.Equal(3, dictRepo.All().Count());
            Assert.Equal(4, langRepo.All().Count());

            langRepo.Delete(german);

            Assert.Equal(3, langRepo.All().Count());
            Assert.Single(dictRepo.All()); //only english-russian stays
        }
    }
}