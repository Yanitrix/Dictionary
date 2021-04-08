using Domain.Dto;
using Domain.Repository;
using Domain.Validation;
using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.Tests.Validation
{
    public class DeleteDictionaryValidatorTests : UowTestBase
    {
        Mock<ILanguageRepository> _langRepo = new Mock<ILanguageRepository>();
        Mock<IDictionaryRepository> _dictRepo = new Mock<IDictionaryRepository>();

        AbstractValidator<DeleteDictionaryCommand> sut;

        public DeleteDictionaryValidatorTests()
        {
            langRepo = _langRepo;
            dictRepo = _dictRepo;
            sut = new DeleteDictionaryValidator(this.uow.Object);
        }

        private void Exists(bool exists = true)
        {
            _dictRepo.Setup(_ => _.ExistsByPrimaryKey(It.IsAny<int>())).Returns(exists);
        }

        [Fact]
        private void DictionaryExists_NoErrors()
        {
            var command = new DeleteDictionaryCommand
            {
                PrimaryKey = 1
            };

            Exists();

            var result = sut.Validate(command);

            Assert.True(result.IsValid);
        }

        [Fact]
        private void DictionaryDoesNotExist_ReturnsError()
        {
            var command = new DeleteDictionaryCommand
            {
                PrimaryKey = 1
            };

            Exists(false);

            var result = sut.Validate(command);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }


    }
}
