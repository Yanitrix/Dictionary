using Service.Validation;
using Data.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace Service.Tests.Validators
{
    public class WordPropertyValidatorTests
    {
        public WordPropertyValidator validator = new WordPropertyValidator();

        public WordProperty entity = new WordProperty
        {
            ID = 0,
            Name = "gender",
            Values = new StringSet{ "feminine" },
        };

        [Fact]
        public void AllMatch_ShouldBeValid()
        {
            var result = validator.Validate(entity);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void IdEmpty_ShoudlBeValid()
        {
            entity.ID = 0;
            var result = validator.Validate(entity);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void IdNotEmpty_ShouldNotBeValid()
        {
            entity.ID = 1;
            var result = validator.Validate(entity);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\t\t")]
        [InlineData("\n\n")] //regex not matched
        public void NameEmpty_ShouldBeInvalid(String name)
        {
            entity.Name = name;
            var result = validator.Validate(entity);
            Assert.False(result.IsValid);
        }

        [Fact]  
        public void NameNotEmpty_ShouldBeValid()
        {
            entity.Name = "s";
            var result = validator.Validate(entity);
            Assert.True(result.IsValid);
        }

        //same rule goes with Value field, thus that's not gonna be checked
    }
}
