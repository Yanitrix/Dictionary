using Domain.Models;
using System;
using System.Collections.Generic;
using Xunit;
using Domain.Repository;
using Persistence.Repository;

namespace Persistence.Tests.Repositories
{
    public class LanguageRepositoryTests : DbContextTestBase
    {
        public ILanguageRepository repo;

        public LanguageRepositoryTests()
        {
            repo = new LanguageRepository(context);
        }

        private void putData()
        {
            var list = new List<Language>
            {
                new (){Name = "japanese"},
                new (){Name = "german"},
                new (){Name = "english"},
                new (){Name = "space space"},
            };

            repo.CreateRange(list.ToArray());
        }

        private void Disconnect()
        {
            this.changeContext();
            repo = new LanguageRepository(this.context);
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
                           Properties = new WordPropertySet
                           {
                               new WordProperty
                               {
                                   Name = "plural form",
                                   Values = new StringSet{"sticks"}
                               }
                           }
                        },

                        new Word
                        {
                            Value = "ox",
                            Properties = new WordPropertySet
                           {
                               new WordProperty
                               {
                                   Name = "plural form",
                                   Values = new StringSet{"oxen"}
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
                           Properties = new WordPropertySet
                           {
                               new WordProperty
                               {
                                   Name = "plural form",
                                   Values = new StringSet{"any japanese words"}
                               }
                           }
                        },

                        new Word
                        {
                            Value = "another japanese word",
                            Properties = new WordPropertySet
                           {
                               new WordProperty
                               {
                                   Name = "plural form",
                                   Values = new StringSet{"other japanese words"}
                               }
                           }

                        }
                    }
                }
            };

            repo.CreateRange(entities);
            Disconnect();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("\t\t")]
        [InlineData("\r\n")]
        [InlineData("")]
        [InlineData("\r")]
        [InlineData("\n")]
        public void GetByName_StringEmpty_ReturnsNull(String name)
        {
            var found = repo.GetByPrimaryKey(name);

            Assert.Null(found);
        }


        [Theory]
        [InlineData("ssds")]
        [InlineData("polish")]
        [InlineData("russian")]
        [InlineData("śöäąćń")]
        [InlineData("English")]
        public void GetByName_NameDoesNotExist_ReturnsNull(String name)
        {
            putData();
            var found = repo.GetByPrimaryKey(name);
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

            var found = repo.GetByPrimaryKey("japanese");

            Assert.NotNull(found);
            Assert.Equal("japanese", found.Name);

            Assert.Empty(found.Words);
        }

        [Theory]
        [InlineData("japanese")]
        [InlineData("german")]
        public void ExistsByName_LanguageExists_ReturnsTrue(String name)
        {
            putData();

            var exists = repo.ExistsByPrimaryKey(name);

            Assert.True(exists);
        }

        [Theory]
        [InlineData("Japanese")]
        [InlineData("German")]
        [InlineData("asdasd")]
        [InlineData("ąśćüö#.ü+")]
        [InlineData("")]//edge cases
        [InlineData(" ")]
        [InlineData("\t\t")]
        [InlineData("\t\n")]
        [InlineData("\r\n")]
        [InlineData("\r")]
        public void ExistsByName_LanguageDoesNoyExistOrCaseDoesNotMatch_ReturnsTrue(String name)
        {
            putData();

            var exists = repo.ExistsByPrimaryKey(name);

            Assert.False(exists);
        }
    }
}
