using Domain.Dto;
using Domain.Repository;
using Domain.Validation;
using FluentValidation;
using Moq;
using System.Collections.ObjectModel;
using Xunit;

namespace Domain.Tests.Validation
{
    public class UpdateMeaningValidatorTests : UowTestBase
    {
        AbstractValidator<UpdateMeaningCommand> sut;
        Mock<IMeaningRepository> _repo = new Mock<IMeaningRepository>();
        Mock<IEntryRepository> _entryRepo = new Mock<IEntryRepository>();

        public UpdateMeaningValidatorTests()
        {
            meaningRepo = _repo;
            entryRepo = _entryRepo;
            sut = new UpdateMeaningValidator(this.uow.Object);
        }

        private UpdateMeaningCommand Command = new()
        {
            Notes = null,
            Value = "value",
            Examples = new Collection<ExampleDto>()
        };

        private void Exists(bool result = true)
        {
            _repo.Setup(_ => _.ExistsByPrimaryKey(It.IsAny<int>())).Returns(result);
        }

        [Fact]
        public void MeaningNotFound_ReturnsError()
        {
            Exists(false);

            var result = sut.Validate(Command);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }

        [Fact]
        public void TryUpdate_MeaningExists_UpdatesProperly()
        {
            Exists();

            var result = sut.Validate(Command);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}
