using Api.Service.Validation;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using Xunit;

namespace Service.Tests.Validators
{
    public class SpeechPartPropertyValidatorTests
    {

        public SpeechPartPropertyValidator validator = new SpeechPartPropertyValidator();

        public SpeechPartProperty entity = new SpeechPartProperty
        {
            ID = 0,
            SpeechPartIndex = 0,
            Name = "gender",
            PossibleValues = new HashSet<String> { "masculine", "feminine", "neuter" }
        };

        [Fact]
        public void AllMatch_ShouldPass()
        {
            Assert.True(validator.Validate(entity).IsValid);
        }

        [Fact]
        public void IdNotEmpty_ShouldFail()
        {
            entity.ID = 1;
            Assert.False(validator.Validate(entity).IsValid);
        }

        [Fact]
        public void SpeechPartIndexNotEmpty_ShouldFail()
        {
            entity.SpeechPartIndex = 2;
            Assert.False(validator.Validate(entity).IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("\r")]
        [InlineData("\t")]
        [InlineData(null)]
        public void NameEmpty_ShouldFail(String name)
        {
            entity.Name = name;
            Assert.False(validator.Validate(entity).IsValid);
        }

        [Fact]
        public void PossibleValuesDontMatchRegex_ShouldFail()
        {
            entity.PossibleValues = new HashSet<String>
            {
                "7jas'd",
            };

            Assert.False(validator.Validate(entity).IsValid);
        }

        [Fact]
        public void PossibleValuesEmpty_ShouldFail()
        {
            entity.PossibleValues = new HashSet<String>();
            Assert.False(validator.Validate(entity).IsValid);
        }

    }
}
