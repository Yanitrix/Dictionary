using Domain.Dto;
using Domain.Repository;
using Domain.Validation;
using FluentValidation;
using Moq;
using System;
using Xunit;

namespace Domain.Tests.Validation
{
    public class DeleteLanguageValidatorTests : UowTestBase
    {
        AbstractValidator<DeleteLanguageCommand> sut;
        Mock<ILanguageRepository> langRepoMock = new Mock<ILanguageRepository>();

        public DeleteLanguageValidatorTests()
        {
            langRepo = langRepoMock;
            sut = new DeleteLanguageValidator(this.uow.Object);
        }

        private DeleteLanguageCommand Command = new()
        {
            PrimaryKey = "name"
        };

        private void Exists(bool exists = true)
        {
            langRepoMock.Setup(_ => _.ExistsByPrimaryKey(It.IsAny<String>())).Returns(exists);
        }

        [Fact]
        public void LangaugeExists_NoErrors()
        {
            var command = Command;

            Exists();

            var result = sut.Validate(command);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void LangaugeDoesNotExist_ReturnsError()
        {
            var command = Command;

            Exists(false);

            var result = sut.Validate(command);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }
    }
}
