using Data.Models;
using Data.Tests;
using System;
using System.Collections.Generic;
using Xunit;
using Service.Repository;

namespace Service.Tests.Repositories
{
    public class ExpressionRepositoryTests : DbContextTestBase
    {
        public IExpressionRepository repo;

        public ExpressionRepositoryTests()
        {
            this.repo = new ExpressionRepository(this.context);
            putData();
        }

        private void putData()
        {
            Expression[] entities =
            {
                new Expression
                {
                    Text = "gegessen sein",
                    Translation = "to be dead and buried"
                },
                new Expression
                {
                    Text = "außer Betrieb sein",
                    Translation = "be out of order"
                },
                new Expression
                {
                    Text = "etwas ins Rollen bringen",
                    Translation = "to set sth in motion"
                },
                new Expression
                {
                    Text = "etwas ins Rollen bringen",
                    Translation = "to get sth underway"
                },
                new Expression
                {
                    Text = "auf etwas großen Wert legen",
                    Translation = "to place a high value on sth"
                },
                new Expression
                {
                    Text = "im Überfluss vorhanden sein",
                    Translation = "to be in plentiful supply"
                },
            };

            repo.CreateRange(entities);
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
            var indexed = new List<Expression>(found);

            Assert.Equal(3, indexed.Count);
            Assert.Equal("gegessen sein", indexed[0].Text);
            Assert.Equal("außer Betrieb sein", indexed[1].Text);
            Assert.Equal("im Überfluss vorhanden sein", indexed[2].Text);

        }

        [Fact]
        public void GetTranslationBySubstring_FoundProper()
        {
            var found = repo.GetByTranslationSubstring("sth");
            var indexed = new List<Expression>(found);

            Assert.Equal(3, indexed.Count);
            Assert.Equal("to set sth in motion", indexed[0].Translation);
            Assert.Equal("to get sth underway", indexed[1].Translation);
            Assert.Equal("to place a high value on sth", indexed[2].Translation);
        }
    }
}
