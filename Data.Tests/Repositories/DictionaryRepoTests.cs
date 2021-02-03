using Data.Repository;
using Data.Models;
using System;
using System.Linq;
using Xunit;

namespace Data.Tests.Repositories
{
    public class DictionaryRepoTests : DbContextTestBase
    {
        IDictionaryRepository repo;

        public DictionaryRepoTests()
        {
            repo = new DictionaryRepository(this.context);
        }

        private void reloadRepo()
        {
            this.changeContext();
            repo = new DictionaryRepository(this.context);
        }

        void putData()
        {
            Dictionary[] entities =
            {
                new Dictionary
                {
                    LanguageIn = new Language
                    {
                        Name = "english"
                    },
                    LanguageOut = new Language
                    {
                        Name = "polish"
                    },
                    LanguageInName = "english",
                    LanguageOutName = "polish"
                },

                new Dictionary
                {
                    LanguageIn = new Language
                    {
                        Name = "german"
                    },
                    LanguageInName = "german",
                    LanguageOutName = "english"
                },

                new Dictionary
                {
                    LanguageInName = "english",
                    LanguageOutName = "german"
                },

                new Dictionary
                {
                    LanguageIn = new Language
                    {
                        Name = "japanese"
                    },
                    LanguageOut = new Language
                    {
                        Name = "zimbabwean"
                    },
                    LanguageInName = "japanese",
                    LanguageOutName = "zimbabwean"
                },

                new Dictionary
                {
                    LanguageIn = new Language
                    {
                        Name = "russian"
                    },
                    LanguageInName = "russian",
                    LanguageOutName = "japanese"
                },

                new Dictionary
                {
                    LanguageIn = new Language
                    {
                        Name = "nor deutsch nor russian"
                    },
                    LanguageOut = new Language
                    {
                        Name = "yes it is"
                    },
                    LanguageInName = "nor deutsch nor russian",
                    LanguageOutName = "yes it is"
                }
            };

            repo.CreateRange(entities);

        }

        [Fact]
        public void GetByIndex_ShouldReturnEntity()
        {
            Dictionary[] entities =
            {
                new Dictionary
                {
                    LanguageIn = new Language
                    {
                        Name = "english"
                    },
                    LanguageOut = new Language
                    {
                        Name = "polish"
                    },
                    LanguageInName = "english",
                    LanguageOutName = "polish"
                },

                new Dictionary
                {
                    LanguageIn = new Language
                    {
                        Name = "german"
                    },
                    LanguageInName = "german",
                    LanguageOutName = "english"
                }
            };

            repo.CreateRange(entities);
            var idx = entities[1].Index;
            reloadRepo();

            var found = repo.GetByIndex(idx);

            Assert.NotNull(found);
            Assert.Equal("german", found.LanguageInName);
            Assert.Equal("english", found.LanguageOutName);
        }

        [Fact]
        public void GetAllByLanguage_ShouldReturnProperEntities()
        {
            putData();

            var withEnglish = repo.GetAllByLanguage("english");
            var withJapanese = repo.GetAllByLanguage("japanese");
            var withRussian = repo.GetAllByLanguage("russian");

            Assert.Equal(3, withEnglish.Count());
            Assert.Equal(2, withJapanese.Count());
            Assert.Single(withRussian);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData("\t\r")]
        [InlineData("\n\r")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\r")]
        [InlineData(null)]
        [InlineData("zwzwzw")]
        public void GetAllByLanguage_LanguageNameEmptyOrNull_ReturnsEmpty(String query)
        {
            putData();

            var found = repo.GetAllByLanguage(query);

            Assert.Empty(found);
        }

        [Theory]
        [InlineData("russian", "japanese")]
        [InlineData("english", "polish")]
        [InlineData("german", "english")]
        public void GetByLanguageInAndOut_ReturnsProperEntity(String langIn, String langOut)
        {
            putData();
            var found = repo.GetByLanguageInAndOut(langIn, langOut);

            Assert.NotNull(found);
            Assert.Equal(langIn, found.LanguageInName);
            Assert.Equal(langOut, found.LanguageOutName);
        }

        [Theory]
        [InlineData("japanese", "russian")]
        [InlineData("polish", "english")]
        [InlineData("\t", null)]
        [InlineData("\r\n", " ")]
        [InlineData("", "\t")]
        public void GetByLanguageInAndOut_ArgsNotFoundOrNullOrEmpty_ReturnsNull(String langIn, String langOut)
        {
            putData();
            var found = repo.GetByLanguageInAndOut(langIn, langOut);

            Assert.Null(found);
        }

        [Theory]
        [InlineData("russian", "japanese")]
        [InlineData("english", "german")]
        [InlineData("japanese", "zimbabwean")]
        public void ExistsByLanguages_NameFound_ReturnsTrue(String langIn, String langOut)
        {
            putData();
            var result = repo.ExistsByLanguages(langIn, langOut);

            Assert.True(result);
        }

        [Theory]
        [InlineData("polish", "english")]
        [InlineData("japanese", "russian")]
        [InlineData("", null)]
        [InlineData(" ", "\t\t\rn")]//on purpose
        [InlineData(null, null)]
        [InlineData("", " ")]
        public void ExistsByLanguages_NameNotFoundOrNullOrEmpty(String langIn, String langOut)
        {
            putData();
            var result = repo.ExistsByLanguages(langIn, langOut);

            Assert.False(result);
        }

        [Theory]
        [InlineData("russian")]
        [InlineData("english")]
        [InlineData("german")]
        public void GetByLanguageIn_NameFound_ReturnsNotNull(String name)
        {
            putData();
            var found = repo.GetAllByLanguageIn(name);

            Assert.NotNull(found);
            foreach (var i in found)
                Assert.Equal(name, i.LanguageInName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("polish")]
        [InlineData("zimbabwean")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData(null)]
        [InlineData(" \t")]
        public void GetByLanguageIn_NotFound_ReturnsEmpty(String name)
        {
            putData();

            var result = repo.GetAllByLanguageIn(name);
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("polish")]
        [InlineData("japanese")]
        [InlineData("zimbabwean")]
        public void GetByLanguageOut_NameFound_ReturnsNotNull(String name)
        {
            putData();
            var found = repo.GetAllByLanguageOut(name);

            Assert.NotNull(found);
            foreach (var i in found)
                Assert.Equal(name, i.LanguageOutName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("russian")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData(null)]
        [InlineData(" \t")]
        public void GetByLanguageOut_NotFound_ReturnsEmpty(String name)
        {
            putData();

            var result = repo.GetAllByLanguageOut(name);
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
