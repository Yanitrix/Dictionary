using Domain.Dto;
using Domain.Models;
using Domain.Repository;
using Domain.Validation;
using FluentValidation;
using Moq;
using System.Linq;
using Xunit;

namespace Domain.Tests.Validation
{
    public class UpdateFreeExpressionValidatorTests : UowTestBase
    {
        AbstractValidator<UpdateFreeExpressionCommand> sut;
        Mock<IFreeExpressionRepository> _repo = new Mock<IFreeExpressionRepository>();
        Mock<IDictionaryRepository> _dictRepo = new Mock<IDictionaryRepository>();

        public UpdateFreeExpressionValidatorTests()
        {
            freeExpressionRepo = _repo;
            dictRepo = _dictRepo;
            sut = new UpdateFreeExpressionValidator(this.uow.Object);
        }

        private UpdateFreeExpressionCommand Command = new()
        {
            ID = 3,
            DictionaryIndex = 12,
            Text = "text",
            Translation = "translation"
        };

        private void Exists(bool exists = true)
        {
            _repo.Setup(_ => _.ExistsByPrimaryKey(It.IsAny<int>())).Returns(exists);
        }

        private void DictionaryExists(bool exists = true)
        {
            _dictRepo.Setup(_ => _.ExistsByPrimaryKey(It.IsAny<int>())).Returns(exists);
        }

        [Fact]
        public void ExpressionExists_UpdatesProperly()
        {
            var command = Command;

            Exists();
            DictionaryExists();

            var result = sut.Validate(command);
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void FreeExpressionExistsButAnotherErrorOccurs_ReturnsError()
        {
            var command = Command;

            Exists();
            DictionaryExists(false);

            var result = sut.Validate(command);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            //Assert.Equal( //TODO validator tests
            //    ValidationErrorMessages.EntityDoesNotExistByForeignKey<FreeExpression, Dictionary>(m => m.Index, command.DictionaryIndex),
            //    result.Errors.First().ErrorMessage
            //);
        }

        [Fact]
        public void TryUpdate_ExpressionDoesNotExist_ReturnsError()
        {
            var command = Command;

            Exists(false);
            DictionaryExists();

            var result = sut.Validate(command);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(
                ValidationErrorMessages.ThereIsNothingToUpdate<FreeExpression>(),
                result.Errors.First().ErrorMessage
            );
        }
    }
}
