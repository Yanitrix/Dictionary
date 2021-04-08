using Moq;
using Domain.Repository;
using System;
using Xunit;
using Domain.Dto;
using FluentValidation;
using Domain.Validation;

namespace Domain.Tests.Validation
{
    public class CreateDictionaryValidatorTests : UowTestBase
    {
        Mock<ILanguageRepository> _langRepo = new Mock<ILanguageRepository>();
        Mock<IDictionaryRepository> _dictRepo = new Mock<IDictionaryRepository>();

        AbstractValidator<CreateDictionaryCommand> sut;

        public CreateDictionaryValidatorTests()
        {
            langRepo = _langRepo;
            dictRepo = _dictRepo;
            sut = new CreateDictionaryValidator(this.uow.Object);
        }

        private CreateDictionaryCommand Command = new()
        {
            LanguageIn = "languageIn",
            LanguageOut = "languageOut"
        };

        private void DuplicateExists()
        {
            _dictRepo.Setup(_ => _.ExistsByLanguages(It.IsAny<String>(), It.IsAny<String>())).Returns(true);
        }

        private void DuplicateDoesNotExist()
        {
            _dictRepo.Setup(_ => _.ExistsByLanguages(It.IsAny<String>(), It.IsAny<String>())).Returns(false);
        }

        private void LanguageInExists(String name)
        {
            _langRepo.Setup(_ => _.ExistsByPrimaryKey(It.Is<String>(x => x == name))).Returns(true);
        }

        private void LanguageInDoesNotExist(String name)
        {
            _langRepo.Setup(_ => _.ExistsByPrimaryKey(It.Is<String>(x => x == name))).Returns(false);
        }

        private void LanguageOutExists(String name) => LanguageInExists(name);

        private void LanguageOutDoesNotExist(String name) => LanguageInDoesNotExist(name);

        [Fact]
        public void LanguagesExistAndNoDuplicates_ShouldAddProperly()
        {
            var command = Command;

            LanguageInExists(command.LanguageIn);
            LanguageOutExists(command.LanguageOut);
            DuplicateDoesNotExist();

            var result = sut.Validate(command);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void AlreadyExists_ShouldReturnError()
        {
            var command = Command;

            LanguageInExists(command.LanguageIn);
            LanguageOutExists(command.LanguageOut);
            DuplicateExists();

            var result = sut.Validate(command);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(result.Errors[0].ErrorMessage, ValidationErrorMessages.DUPLICATE_DICTIONARY_DESC);
        }

        [Fact]
        public void BothLanguagesDontExist_ShouldReturnError()
        {
            var command = Command;

            LanguageInDoesNotExist(command.LanguageIn);
            LanguageOutDoesNotExist(command.LanguageOut);

            var result = sut.Validate(command);

            Assert.False(result.IsValid);
            //Assert.Single(result.Errors);
            //Assert.Equal( //todo validators
            //    result.Errors[0].ErrorMessage,
            //    ValidationErrorMessages.LanguagesNotFoundDesc(command.LanguageIn, command.LanguageOut)
            //);
        }

        [Fact]
        public void OneLanguageDoesNotExist_ShouldReturnError()
        {
            var command = Command;

            LanguageInExists(command.LanguageIn);
            LanguageOutDoesNotExist(command.LanguageOut);

            var result = sut.Validate(command);

            Assert.False(result.IsValid);
            //Assert.Single(result.Errors); //todo validators
            //Assert.Equal(
            //    result.Errors[0].ErrorMessage,
            //    ValidationErrorMessages.LanguagesNotFoundDesc(command.LanguageIn, command.LanguageOut)
            //);
        }

    }
}
