using Domain.Models;
using Domain.Repository;
using Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Persistence.Tests.Repositories
{
    public class ExampleRepositoryTests : DbContextTestBase
    {
        IExampleRepository repo;

        public ExampleRepositoryTests()
        {
            this.repo = new ExampleRepository(this.context);
        }

        //TODO do some more tests
        [Fact]
        public void GetByTextAndDictionary_ReturnsProperExpressions()
        {
            var dict1 = new Dictionary
            {
                LanguageIn = new Language { Name = "in" },
                LanguageOut = new Language { Name = "out" },

                Entries = new HashSet<Entry>
                {
                    new Entry
                    {
                        Word = new Word
                        {
                            SourceLanguageName = "in",
                            Value = "value",
                        },

                        Meanings = new HashSet<Meaning>
                        {
                            new Meaning
                            {
                                Value = "val",
                                Examples = new HashSet<Example>
                                {
                                    new Example
                                    {
                                        Text = "dict1.text1",
                                        Translation = "dict1.translation1"
                                    },

                                    new Example
                                    {
                                        Text = "dict1.text2",
                                        Translation = "dict1.translation2"
                                    },

                                    new Example
                                    {
                                        Text = "dict1.text3",
                                        Translation = "dict1.translation3"
                                    },
                                }
                            }
                        }
                    },
                }
            };

            var dict2 = new Dictionary
            {
                LanguageInName = "out",
                LanguageOutName = "in",

                Entries = new HashSet<Entry>
                {
                    new Entry
                    {
                        Word = new Word
                        {
                            SourceLanguageName = "out",
                            Value = "value",
                        },

                        Meanings = new HashSet<Meaning>
                        {
                            new Meaning
                            {
                                Value = "val",
                                Examples = new HashSet<Example>
                                {
                                    new Example
                                    {
                                        Text = "dict2.text1",
                                        Translation = "dict2.translation1"
                                    },

                                    new Example
                                    {
                                        Text = "dict2.text3",
                                        Translation = "dict2.translation3"
                                    },
                                }
                            }
                        }
                    },
                }
            };

            context.Dictionaries.Add(dict1);
            context.Dictionaries.Add(dict2);
            context.SaveChanges();

            changeContext();
            repo = new ExampleRepository(this.context);

            var found = repo.GetByDictionaryAndTextSubstring(dict2.Index, "text");

            Assert.NotEmpty(found);
            Assert.Equal(2, found.Count());

            Assert.Equal("dict2.translation1", found.First().Translation);
            Assert.Equal("dict2.translation3", found.Last().Translation);
        }
    }
}
