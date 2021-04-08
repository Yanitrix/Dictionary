using Domain.Dto;
using Domain.Models;
using Domain.Queries;
using Domain.Repository;
using Domain.Validation;
using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Tests.Validation
{
    public class CreateWordValidatorTests : UowTestBase
    {
        AbstractValidator<CreateWordCommand> sut;
        Mock<IWordRepository> _wordRepo = new Mock<IWordRepository>();
        Mock<ILanguageRepository> _langRepo = new Mock<ILanguageRepository>();

        public CreateWordValidatorTests()
        {
            wordRepo = _wordRepo;
            langRepo = _langRepo;
            sut = new CreateWordValidator(this.uow.Object);
        }

        private CreateWordCommand Command => new()
        {
            Value = "value",
            SourceLanguageName = "polish",
            Properties = new HashSet<WordPropertyDto>()
            {
                new()
                {
                    Name = "gender",
                    Values = new List<String>{ "masculine", "feminine" },
                },

                new()
                {
                    Name = "plural form",
                    Values = new List<String>{ "przeręble" }
                },

                new()
                {
                    Name = "genitive form",
                    Values = new List<String>{ "przerębla", "przerębli" }
                }
            }
        };

        private void LanguageExists(bool exists = true)
        {
            _langRepo.Setup(_ => _.ExistsByPrimaryKey(It.IsAny<String>())).Returns(exists);
        }

        [Fact]
        public void LanguageExists_NoError()
        {
            LanguageExists(); //source language exists

            var dto = Command;

            var result = sut.Validate(dto);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void LanguageDoesNotExist_ReturnsError()
        {
            LanguageExists(false);

            var dto = Command;

            var result = sut.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Single (result.Errors);
        }


    }
}
