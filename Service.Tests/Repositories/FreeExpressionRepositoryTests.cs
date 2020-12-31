using Data.Models;
using System;
using System.Collections.Generic;
using Xunit;
using Service.Repository;
using System.Linq;

namespace Service.Tests.Repositories
{
    public class FreeExpressionRepositoryTests : DbContextTestBase
    {
        public IFreeExpressionRepository repo;

        public FreeExpressionRepositoryTests()
        {
            this.repo = new FreeExpressionRepository(this.context);
            putData();
        }

        private void putData()
        {
            var dictionary = new Dictionary
            {
                LanguageIn = new Language
                {
                    Name = "german"
                },

                LanguageOut = new Language
                {
                    Name = "english"
                },
            };

            FreeExpression[] entities =
            {
                new FreeExpression
                {
                    Text = "gegessen sein",
                    Translation = "to be dead and buried"
                },
                new FreeExpression
                {
                    Text = "außer Betrieb sein",
                    Translation = "be out of order"
                },
                new FreeExpression
                {
                    Text = "etwas ins Rollen bringen",
                    Translation = "to set sth in motion"
                },
                new FreeExpression
                {
                    Text = "etwas ins Rollen bringen",
                    Translation = "to get sth underway"
                },
                new FreeExpression
                {
                    Text = "auf etwas großen Wert legen",
                    Translation = "to place a high value on sth"
                },
                new FreeExpression
                {
                    Text = "im Überfluss vorhanden sein",
                    Translation = "to be in plentiful supply"
                },
            };

            dictionary.FreeExpressions = entities.ToList();
            context.Dictionaries.Add(dictionary);
            context.SaveChanges();
        }

        [Theory]
        [InlineData("")]
        [InlineData("\r")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData(" ")]
        public void GetBySubstring_SubstringEmpty_ReturnsEmpty(String query)
        {
            var foundText = repo.GetByTextSubstring(query);
            var foundTranslation = repo.GetByTranslationSubstring(query);

            Assert.Empty(foundText);
            Assert.Empty(foundTranslation);
        }

        [Fact]
        public void GetBySubstring_SubstringNull_ReturnsEmpty()
        {
            //test both because why not

            var foundText = repo.GetByTextSubstring(null);
            var foundTranslation = repo.GetByTranslationSubstring(null);

            Assert.Empty(foundText);
            Assert.Empty(foundTranslation);
        }

        [Fact]
        public void GetBySubstring_NotFound_ReturnsEmpty()
        {
            var foundText = repo.GetByTextSubstring("machen");
            var foundTranslation = repo.GetByTranslationSubstring("split");

            Assert.Empty(foundText);
            Assert.Empty(foundTranslation);
        }

        [Fact]
        public void GetTextBySubstring_FoundsProper()
        {
            var found = repo.GetByTextSubstring("sein");

            Assert.Equal(3, found.Count());
            Assert.Contains(found, f => String.Equals(f.Text, "gegessen sein", StringComparison.OrdinalIgnoreCase));
            Assert.Contains(found, f => String.Equals(f.Text, "außer Betrieb sein", StringComparison.OrdinalIgnoreCase));
            Assert.Contains(found, f => String.Equals(f.Text, "im Überfluss vorhanden sein", StringComparison.OrdinalIgnoreCase));

        }

        [Fact]
        public void GetTranslationBySubstring_FoundProper()
        {
            var found = repo.GetByTranslationSubstring("sth");

            Assert.Equal(3, found.Count());
            Assert.Contains(found, f => String.Equals(f.Translation, "to set sth in motion", StringComparison.OrdinalIgnoreCase));
            Assert.Contains(found, f => String.Equals(f.Translation, "to get sth underway", StringComparison.OrdinalIgnoreCase));
            Assert.Contains(found, f => String.Equals(f.Translation, "to place a high value on sth", StringComparison.OrdinalIgnoreCase));
        }
    }
}
