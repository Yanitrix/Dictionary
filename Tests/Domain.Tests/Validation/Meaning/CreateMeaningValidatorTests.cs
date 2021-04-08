using Domain.Dto;
using Domain.Repository;
using Domain.Validation;
using FluentValidation;
using Moq;
using System.Collections.ObjectModel;
using Xunit;

namespace Domain.Tests.Validation
{
    public class CreateMeaningValidatorTests : UowTestBase
    {
        AbstractValidator<CreateMeaningCommand> sut;
        Mock<IMeaningRepository> _repo = new Mock<IMeaningRepository>();
        Mock<IEntryRepository> _entryRepo = new Mock<IEntryRepository>();

        public CreateMeaningValidatorTests()
        {
            meaningRepo = _repo;
            entryRepo = _entryRepo;
            sut = new CreateMeaningValidator(this.uow.Object);
        }

        private CreateMeaningCommand Command = new()
        {
            EntryID = 1,
            Notes = null,
            Value = "value",
            Examples = new Collection<ExampleDto>()
        };

        private void EntryExists(bool result = true)
        {
            _entryRepo.Setup(_ => _.ExistsByPrimaryKey(It.IsAny<int>())).Returns(result);
        }

        [Fact]
        public void EntryExists_AddsProperly()
        {
            EntryExists();

            var result = sut.Validate(Command);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void EntryDoesNotExist_ReturnsError()
        {
            EntryExists(false);

            var result = sut.Validate(Command);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }
    }
}
