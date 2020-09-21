using Api.Service;
using Api.Service.Validation;
using Data.Models;
using Data.Tests;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Service.Tests.Repositories
{
    public class LanguageRepositoryTests : DbContextTestBase
    {
        public LanguageRepository repo;

        public LanguageRepositoryTests()
        {
            repo = new LanguageRepository(context);
            //putData();
        }

        private void putData()
        {
            var list = new List<Language>
            {
                new Language{Name = "japanese"},
                new Language{Name = "german"},
                new Language{Name = "english"},
            };

            repo.CreateRange(list.ToArray());
        }

        private void putData1()
        {
            Language[] entities =
            {
                new Language
                {
                    Name = "english",
                    Words = new HashSet<Word>
                    {
                        new Word
                        {
                           Value = "stick",
                           Properties = new List<WordProperty>
                           {
                               new WordProperty
                               {
                                   Name = "plural form",
                                   Values = new HashSet<String>{"sticks"}
                               }
                           }
                        },

                        new Word
                        {
                            Value = "ox",
                            Properties = new List<WordProperty>
                           {
                               new WordProperty
                               {
                                   Name = "plural form",
                                   Values = new HashSet<String>{"oxen"}
                               }
                           }

                        }
                    }
                },

                new Language
                {
                    Name = "japanese",
                    Words = new HashSet<Word>
                    {
                        new Word
                        {
                           Value = "any japanese word",
                           Properties = new List<WordProperty>
                           {
                               new WordProperty
                               {
                                   Name = "plural form",
                                   Values = new HashSet<String>{"any japanese words"}
                               }
                           }
                        },

                        new Word
                        {
                            Value = "another japanese word",
                            Properties = new List<WordProperty>
                           {
                               new WordProperty
                               {
                                   Name = "plural form",
                                   Values = new HashSet<String>{"other japanese words"}
                               }
                           }

                        }
                    }
                }
            };

            repo.CreateRange(entities);
            //detaching so that they must be loaded from DB
            repo.Detach(entities[0]);
            repo.Detach(entities[1]);
        }

        [Fact]
        public void GetByName_StringNull_ReturnsNull()
        {
            String name = null;
            var found = repo.GetByName(name);

            Assert.Null(found);
        }


        [Theory]
        [InlineData("ssds")]
        [InlineData("polish")]
        [InlineData("russian")]
        [InlineData("śöäąć")]
        public void GetByName_NameDoesNotExist_ReturnsNull(String name)
        {
            putData();
            var found = repo.GetByName(name);
            Assert.Null(found);
        }

        [Theory]
        [InlineData("japanese")]
        [InlineData("english")]
        public void GetByNameWithWords_NameExists_ReturnsEntityWithLoadedWords(String name)
        {
            putData1();

            var found = repo.GetByNameWithWords(name);
            var words = new List<Word>(found.Words);

            Assert.NotNull(found);
            Assert.Equal(typeof(Language), found.GetType());
            Assert.Equal(name, found.Name);

            Assert.NotEmpty(found.Words);
            Assert.NotEmpty(words[0].Properties);
            Assert.NotEmpty(words[1].Properties);
        }

        
        [Fact]
        public void GetByName_ReturnsEntityWithoutLoadedWords()
        {
            putData1();

            var found = repo.GetByName("japanese");

            Assert.NotNull(found);
            Assert.Equal("japanese", found.Name);

            Assert.Empty(found.Words);
        }

    }
}
