using Api.Service.Validation;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Service.Tests.Validators
{
    public class LanguageValidatorTests
    {
        public LanguageValidator validator = new LanguageValidator();

        [Theory]
        [InlineData("polski język migowy")]
        [InlineData("ssh'as")]
        [InlineData("87snasd")]
        [InlineData("Русский язык")]
        public void IncorrectRegex_ShouldNotBeValid(String name)
        {
            var entity = new Language
            {
                Name = name
            };

            var result = validator.Validate(entity);

            Assert.False(result.IsValid);
        }
        
        [Theory]
        [InlineData("polish")]
        [InlineData("urdu")]
        [InlineData("chiński")]
        [InlineData("Русский")]
        [InlineData("hashdąśjashd")]
        public void CorrectRegex_ShouldBeValid(String name)
        {
            var entity = new Language
            {
                Name = name
            };

            var result = validator.Validate(entity);

            Assert.True(result.IsValid);
        }

    }
}
