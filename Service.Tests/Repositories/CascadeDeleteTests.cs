using Data.Models;
using Data.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Service.Repository;

namespace Service.Tests.Repositories
{
    public class CascadeDeleteTests : DbContextTestBase
    {
        ILanguageRepository langRepo;
        IWordRepository wordRepo;
        IDictionaryRepository dictRepo;
        IEntryRepository entryRepo;
        IMeaningRepository meaningRepo;
        IExpressionRepository expressionRepo;


        public CascadeDeleteTests()
        {
            langRepo = new LanguageRepository(this.context);
            wordRepo = new WordRepository(this.context);
            dictRepo = new DictionaryRepository(this.context);
            entryRepo = new EntryRepository(this.context);
            meaningRepo = new MeaningRepository(this.context);
            expressionRepo = new ExpressionRepository(this.context);
        }

        /*
         to delete:
        dictionary with expressions, //done
        meaning with expressions,  //done
        meaning and dictionary with same expressions,  //done
        expressions that's inside a dictionary and also inside a meaning of one of the dictionary entries  //done

        entry with meanings, //done
        dictionary with entries, //done
        language with words //done

        word with entries, //done
        language with dictionaries //done
        */
        [Fact]
        public void DeleteDictionaryWithExpressions()
        {
            var dict = new Dictionary
            {
                LanguageInName = "sds",
                LanguageOutName = "sdasda",
                FreeExpressions = new List<Expression>
                {
                    new Expression
                    {
                        Text = "text",
                        Translation = "translation"
                    },

                    new Expression
                    {
                        Text = "text",
                        Translation = "Transllation"
                    },

                    new Expression
                    {
                        Text = "text",
                        Translation = "translation"
                    },

                    new Expression
                    {
                        Text = "text",
                        Translation = "translation"
                    },

                }
            };

            var exp = new Expression
            {
                DictionaryIndex = 12,
                Text = "sdasda",
                Translation = "sdasd"
            };

            dictRepo.Create(dict);
            expressionRepo.Create(exp);

            Assert.NotEmpty(dictRepo.All());
            Assert.Equal(5, expressionRepo.All().Count());

            dictRepo.Detach(dict);
            dictRepo.Delete(dict);

            Assert.Empty(dictRepo.All());
            Assert.Single(expressionRepo.All());

        }

        [Fact]
        public void DeleteMeaningWithExpressions()
        {
            var meaning = new Meaning
            {
                Examples = new List<Expression>
                {
                    new Expression
                    {
                        Text = "text",
                        Translation = "translation"
                    },

                    new Expression
                    {
                        Text = "text",
                        Translation = "Transllation"
                    },

                    new Expression
                    {
                        Text = "text",
                        Translation = "translation"
                    },
                }
            };

            Expression[] expressions =
            {
                new Expression
                {
                    MeaningID = 12,
                    Text = "a",
                    Translation = "b"
                },

                new Expression
                {
                    MeaningID = 12,
                    Text = "a",
                    Translation = "b"
                }
            };

            meaningRepo.Create(meaning);
            expressionRepo.CreateRange(expressions);

            Assert.NotEmpty(meaningRepo.All());
            Assert.Equal(5, expressionRepo.All().Count());

            meaningRepo.Detach(meaning);
            meaningRepo.Delete(meaning);

            Assert.Empty(meaningRepo.All());
            Assert.Equal(2, expressionRepo.All().Count());
        }

        [Fact]
        public void DeleteMeaningAndDictionaryWithSameExpression_ShouldBeNoErrors()
        {
            Expression[] exprs =
            {
                new Expression
                {
                    Text = "ss",
                    Translation = "sdsds"
                },

                new Expression
                {
                    Text = "ss",
                    Translation = "sdsds"
                },

                new Expression
                {
                    Text = "ss",
                    Translation = "sdsds"
                },

                new Expression
                {
                    Text = "ss",
                    Translation = "sdsds"
                },
            };

            var dict = new Dictionary
            {
                LanguageInName = "sds",
                LanguageOutName = "sdasda",
                FreeExpressions = new HashSet<Expression>
                {
                    new Expression
                    {
                        Text = "dd",
                        Translation = "asdasd"
                    },

                    new Expression
                    {
                        Text = "dfsdf",
                        Translation = "sadsdsds"
                    },
                }
            };

            var meaning = new Meaning
            {
                Examples = new HashSet<Expression>
                {
                    new Expression
                    {
                        Text = "sadasdas",
                        Translation = "hahsdhasd"
                    }
                }
            };

            dictRepo.Create(dict);
            meaningRepo.Create(meaning);

            exprs[0].DictionaryIndex = dict.Index;
            exprs[1].DictionaryIndex = dict.Index;
            exprs[0].MeaningID = meaning.ID;
            exprs[1].MeaningID = meaning.ID;

            expressionRepo.CreateRange(exprs);

            //so now:
            //dictionary has 4 expressions,
            //meaning has 3 expressions
            //2 of them are common
            //2 expressions are free

            meaningRepo.Delete(meaning);
            dictRepo.Delete(dict);

            Assert.Empty(meaningRepo.All());

            Assert.Equal(2, expressionRepo.All().Count());
        }

        [Fact]
        public void DeleteDictionaryWithMeaning_BothHaveSameExpression()
        {
            Expression[] expressions =
            {
                new Expression
                {
                    Text = "text1",
                    Translation = "trans1"
                },

                new Expression
                {
                    Text = "text2",
                    Translation = "trans2"
                },

                new Expression
                {
                    Text = "text3",
                    Translation = "trans3"
                },
            };

            expressionRepo.CreateRange(expressions);

            var dict = new Dictionary
            {
                LanguageInName = "in",
                LanguageOutName = "out",
                FreeExpressions = new HashSet<Expression>
                {
                    expressions[0],
                    expressions[1]
                },

                Entries = new HashSet<Entry>
                {
                    new Entry
                    {
                        WordID = 1,
                        Meanings = new HashSet<Meaning>
                        {
                            new Meaning
                            {
                                Examples = new HashSet<Expression>
                                {
                                    expressions[0],
                                    expressions[1]
                                }
                            }
                        }
                    }
                }

            };

            dictRepo.Create(dict);

            Assert.Single(dictRepo.All());
            Assert.Equal(3, expressionRepo.All().Count());
            Assert.Single(meaningRepo.All());

            dictRepo.Delete(dict);

            Assert.Empty(dictRepo.All());
            Assert.Empty(meaningRepo.All());
            Assert.Single(expressionRepo.All());
        }

        [Fact]
        public void DeleteEntryWithMeanings()
        {
            Entry[] entries =
            {
                new Entry
                {
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
                }
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

            entryRepo.CreateRange(entries);
            meaningRepo.CreateRange(meanings);

            Assert.Equal(2, entryRepo.All().Count());
            Assert.Equal(7, meaningRepo.All().Count());

            entryRepo.Delete(entries[0]);

            Assert.Single(entryRepo.All());
            Assert.Equal(5, meaningRepo.All().Count());

            var indexed = new List<Meaning>(meaningRepo.All());

            //Assert.Equal("entry2.meaning1", indexed[0].Value);
            //Assert.Equal("entry2.meaning2", indexed[1].Value);
            //Assert.Equal("entry2.meaning3", indexed[2].Value);
            //Assert.Equal("free2", indexed[3].Value);
            //Assert.Equal("free1", indexed[4].Value);
        }

        [Fact]
        public void DeleteDictionaryWithEntries()
        {
            var dict = new Dictionary
            {
                LanguageInName = "in",
                LanguageOutName = "out",
                Entries = new HashSet<Entry>
                {
                    new Entry
                    {
                        WordID = 1,
                    },

                    new Entry
                    {
                        WordID = 2
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
                        WordID = 3,
                    },

                    new Entry
                    {
                        WordID = 4
                    },

                    new Entry
                    {
                        WordID = 5
                    },

                    new Entry
                    {
                        WordID = 6
                    }
}
            };

            Entry[] entries =
            {
                new Entry
                {
                    WordID = 12
                }

            };

            dictRepo.Create(dict);
            dictRepo.Create(dict1);
            entryRepo.CreateRange(entries);

            Assert.Equal(2, dictRepo.All().Count());
            Assert.Equal(7, entryRepo.All().Count());

            dictRepo.Delete(dict1);

            Assert.Single(dictRepo.All());
            Assert.Equal(3, entryRepo.All().Count());
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
                    SourceLanguageName = "russian",
                    Value = " skdjaksjdasd"
                },

                new Word
                {
                    SourceLanguageName = "english",
                    Value = "sadsdas"
                }
            };

            langRepo.Create(lang1);
            langRepo.Create(lang2);
            wordRepo.CreateRange(words);

            Assert.Equal(2, langRepo.All().Count());
            Assert.Equal(7, wordRepo.All().Count());

            langRepo.Delete(lang1);

            Assert.Single(langRepo.All());
            Assert.Equal(4, wordRepo.All().Count());

            langRepo.Delete(lang2);

            Assert.Empty(langRepo.All());
            Assert.Equal(2, wordRepo.All().Count());
        }

        [Fact]
        public void DeleteWordWithEntries()
        {
            var dict = new Dictionary
            {
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
                                Examples = new HashSet<Expression>
                                {
                                    new Expression
                                    {
                                        Text = "to get the stick",
                                        Translation  = "den Stock bekommen"
                                    },

                                    new Expression
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
                                Examples = new HashSet<Expression>
                                {
                                    new Expression
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
                                Examples = new HashSet<Expression>
                                {
                                    new Expression
                                    {
                                        Text = "to be in train",
                                        Translation = "im Gange sein"
                                    },

                                    new Expression
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

            var germanInDB = langRepo.GetByName("German");
            langRepo.Delete(german);

            Assert.Equal(3, langRepo.All().Count());
            Assert.Single(dictRepo.All()); //only english-russian stays
        }
    }
}
