﻿using Api.Service;
using Data.Models;
using Data.Tests;
using System;
using System.Linq;
using Xunit;

namespace Service.Tests.Repositories
{
    public class DictionaryRepoTests : DbContextTestBase
    {
        IDictionaryRepository repo;

        public DictionaryRepoTests()
        {
            repo = new DictionaryRepository(this.context);
        }

        void putData()
        {
            Dictionary[] entities =
            {
                new Dictionary
                {
                    LanguageInName = "english",
                    LanguageOutName = "polish"
                },

                new Dictionary
                {
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
                    LanguageInName = "japanese",
                    LanguageOutName = "zimbabwean"
                },

                new Dictionary
                {
                    LanguageInName = "russian",
                    LanguageOutName = "japanese"
                },

                new Dictionary
                {
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
                    LanguageInName = "english",
                    LanguageOutName = "polish"
                },

                new Dictionary
                {
                    LanguageInName = "german",
                    LanguageOutName = "english"
                }
            };

            repo.CreateRange(entities);
            var idx = entities[1].Index;
            repo.Detach(entities[0]);
            repo.Detach(entities[1]);

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

    }
}
