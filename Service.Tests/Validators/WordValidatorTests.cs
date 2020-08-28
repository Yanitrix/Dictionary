﻿using Api.Service.Validation;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Service.Tests.Validators
{
    public class WordValidatorTests
    {
        public WordValidator validator = new WordValidator();

        private Word entity = new Word //constructed so that it passes
        {
            ID = 0,
            SourceLanguageName = "english",
            SpeechPartName = "noun",
            Value = "dog"
        };

        [Theory]
        [InlineData(-1)]
        [InlineData(23)]
        [InlineData(7712)]
        [InlineData(-3)]
        public void IdNotEmpty_ShouldBeInvalid(int id)
        {
            entity.ID = id;

            var result = validator.Validate(entity);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void IdEmpty_ShouldBeValid()
        {
            entity.ID = 0;

            var result = validator.Validate(entity);

            Assert.True(result.IsValid);
        }


        //in two method below the regex correctness isn't checked because that's done in LanguageValidatorTests.
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r")]
        public void SourceLanguageNameEmpty_ShouldBeInvalid(String name)
        {
            entity.SourceLanguageName = name;

            var result = validator.Validate(entity);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("hstus")]
        [InlineData("english")]
        [InlineData("jjjsh")]
        [InlineData("pomasd")]
        public void SourceLanguageNamePresent_ShouldBeValid(String name)
        {
            entity.SourceLanguageName = name;

            var result = validator.Validate(entity);

            Assert.True(result.IsValid);
        }

        //in two method below the regex correctness isn't checked because that's done in SpeechPartValidatorTests.
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r")]
        public void SpeechPartNameEmpty_Invalid(String name)
        {
            entity.SourceLanguageName = name;

            var result = validator.Validate(entity);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("noun")]
        [InlineData("oneword")]
        [InlineData("adjektiv")]
        public void SpeechPartNamePresent_ShouldBeValid(String name)
        {
            entity.SourceLanguageName = name;

            var result = validator.Validate(entity);

            Assert.True(result.IsValid);
        }
    }
}
