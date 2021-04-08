using Domain.Dto;
using Domain.Repository;
using Domain.Validation;
using FluentValidation;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace Domain.Tests.Validation
{
    public class CreteaLanguageValidatorTests : UowTestBase
    {
        AbstractValidator<CreateLanguageCommand> sut;
        Mock<ILanguageRepository> langRepoMock = new Mock<ILanguageRepository>();

        public CreteaLanguageValidatorTests()
        {
            langRepo = langRepoMock;
            sut = new CreateLanguageValidator(this.uow.Object);
        }

        private CreateLanguageCommand Command = new()
        {
            Name = "name"
        };

        private void DuplicateExists(bool exists = true)
        {
            langRepoMock.Setup(_ => _.ExistsByPrimaryKey(It.IsAny<String>())).Returns(exists);
        }

        [Fact]
        public void LanguageExists_ReturnsError()
        {
            var command = Command;

            DuplicateExists();

            var result = sut.Validate(command);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(
                ValidationErrorMessages.DUPLICATE_LANGUAGE_DESC,
                result.Errors.First().ErrorMessage
            );
        }

        [Fact]
        public void LanguageDoesNotExist_AddsProperly()
        {
            var command = Command;

            DuplicateExists(false);

            var result = sut.Validate(command);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}
