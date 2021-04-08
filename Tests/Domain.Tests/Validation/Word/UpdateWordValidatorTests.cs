using Domain.Dto;
using Domain.Repository;
using Domain.Validation;
using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Domain.Tests.Validation
{
    public class UpdateWordValidatorTests : UowTestBase
    {
        AbstractValidator<UpdateWordCommand> sut;
        Mock<IWordRepository> _wordRepo = new Mock<IWordRepository>();
        Mock<ILanguageRepository> _langRepo = new Mock<ILanguageRepository>();

        public UpdateWordValidatorTests()
        {
            wordRepo = _wordRepo;
            langRepo = _langRepo;
            sut = new UpdateWordValidator(this.uow.Object);
        }

        private UpdateWordCommand Command => new()
        {
            ID = 13,
            Value = "value",
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

        private void Exists(bool exists = true)
        {
            _wordRepo.Setup(_ => _.ExistsByPrimaryKey(It.IsAny<int>())).Returns(exists);
        }

        [Fact]
        private void WordExists_ReturnsNoErrors()
        {
            var command = Command;

            Exists();

            var result = sut.Validate(command);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        private void WordDoesNotExist_ReturnsError()
        {
            var command = Command;

            Exists(false);

            var result = sut.Validate(command);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }
    }
}
