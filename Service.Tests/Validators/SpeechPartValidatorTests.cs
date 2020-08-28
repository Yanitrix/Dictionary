using Api.Service.Validation;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Service.Tests.Validators
{
    public class SpeechPartValidatorTests
    {
        public SpeechPartValidator validator = new SpeechPartValidator();

        public SpeechPart entity = new SpeechPart
        {
            Index = 0,
            LanguageName = "english",
            Name = "noun",
            Properties = new HashSet<SpeechPartProperty>
            {
                new SpeechPartProperty
                {
                    ID = 0,
                    SpeechPartIndex = 0,
                    Name = "gender",
                    PossibleValues = new HashSet<String> { "masculine", "feminine", "neuter" }
                },

                new SpeechPartProperty
                {
                    ID = 0,
                    SpeechPartIndex = 0,
                    Name = "countability",
                    PossibleValues = new HashSet<String> { "countable", "uncountable" }
                },
            }
        };

        [Fact]
        public void AllMatch_ShouldPass()
        {
            Assert.True(validator.Validate(entity).IsValid);
        }

        [Fact]
        public void PropertiesEmpty_ShouldFail()
        {
            entity.Properties = new HashSet<SpeechPartProperty>();
            Assert.False(validator.Validate(entity).IsValid);
        }

    }
}
