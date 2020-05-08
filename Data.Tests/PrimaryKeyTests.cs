using Dictionary_MVC.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Data.Tests
{
    public class PrimaryKeyTests : DbContextTestBase
    {

        [Theory]
        [InlineData("german", "german", true)]
        [InlineData("English", "english", true)]
        [InlineData("polish", "japanese", false)]
        [InlineData("japanese", "afrikaans", false)]
        public void LanguagePrimaryKeyViolation_ShouldThrowException(String languageOriginal, String languageTested, bool primaryKeyViolated)
        {
            Language original = new Language
            {
                Name = languageOriginal.ToLowerInvariant(),
            };

            context.Languages.Add(original);

            //Action action = () =>
            //{
            //    Language tested = new Language
            //    {
            //        Name = languageTested.ToLowerInvariant(),
            //    };

            //    context.Languages.Add(tested);
            //    context.SaveChanges();

            //    //context.Languages.Add(german1);
            //    //context.SaveChanges();
            //};

            Exception ex = null;

            try
            {
                Language tested = new Language
                {
                    Name = languageTested.ToLowerInvariant(),
                };

                context.Languages.Add(tested);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                ex = e;
            }

            //var ex = Assert.Throws<InvalidOperationException>(action);

            var actual = ex == null ? false : typeof(InvalidOperationException) == ex.GetType();

            Assert.Equal(primaryKeyViolated, actual);
        }

        [Fact]
        public void DictionaryPrimaryKeyViolation_ShouldThrowException()
        {
            Language german = new Language
            {
                Name = "german",
            };

            Language english = new Language
            {
                Name = "english",
            };

            Dictionary d = new Dictionary
            {
                LanguageIn = german,
                LanguageOut = english,
            };

            var d1 = new Dictionary
            {
                LanguageIn = german,
                LanguageOut = english,
            };

            context.Dictionaries.Add(d);

            Assert.Throws<InvalidOperationException>(() =>
            {
                context.Add(d1);
            });
        }
    }
}
