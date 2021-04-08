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
    public class CreateFreeExpressionValidatorTests : UowTestBase
    {
        AbstractValidator<CreateFreeExpressionCommand> sut;
        Mock<IFreeExpressionRepository> _repo = new Mock<IFreeExpressionRepository>();
        Mock<IDictionaryRepository> _dictRepo = new Mock<IDictionaryRepository>();

        public CreateFreeExpressionValidatorTests()
        {
            freeExpressionRepo = _repo;
            dictRepo = _dictRepo;
            sut = new CreateFreeExpressionValidator(this.uow.Object);
        }

        private CreateFreeExpressionCommand Command = new()
        {
            DictionaryIndex = 12,
            Text = "text",
            Translation = "translation"
        };

        [Fact]
        public void DictionaryExists_AddsProperly()
        {
            _dictRepo.Setup(_ => _.ExistsByPrimaryKey(It.Is<int>(i => i == Command.DictionaryIndex))).Returns(true);

            var result = sut.Validate(Command);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void DictionaryDoesNotExist_ReturnsError()
        {
            _dictRepo.Setup(_ => _.ExistsByPrimaryKey(It.Is<int>(i => i == Command.DictionaryIndex))).Returns(false);

            var result = sut.Validate(Command);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            //Assert.Equal( //TODO validator tests
            //    ValidationErrorMessages.EntityDoesNotExistByForeignKey<FreeExpression, Dictionary>(m => m.Index, Command.DictionaryIndex),
            //    result.Errors.First().ErrorMessage
            //);
        }
    }
}
