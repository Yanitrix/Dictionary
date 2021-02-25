using Domain.Repository;
using Domain.Models;
using Moq;
using System.Linq;
using Xunit;

namespace Service.Tests.Service
{
    public class MeaningServiceTests : UowTestBase
    {
        IMeaningService service;
        Mock<IMeaningRepository> _repo = new Mock<IMeaningRepository>();
        Mock<IEntryRepository> _entryRepo = new Mock<IEntryRepository>();

        public MeaningServiceTests()
        {
            meaningRepo = _repo;
            entryRepo = _entryRepo;
            service = new MeaningService(this.uow.Object, this.mapper);
        }

        private void Exists(bool result = true)
        {
            _repo.Setup(_ => _.ExistsByID(It.IsAny<int>())).Returns(result);
        }

        private void EntryExists(bool result = true)
        {
            _entryRepo.Setup(_ => _.ExistsByID(It.IsAny<int>())).Returns(result);
        }

        private void ShouldAdd()
        {
            _repo.Verify(_ => _.Create(It.IsAny<Meaning>()), Times.Once);
        }

        private void ShouldNotAdd()
        {
            _repo.Verify(_ => _.Create(It.IsAny<Meaning>()), Times.Never);
        }

        private void ShouldUpdate()
        {
            _repo.Verify(_ => _.Update(It.IsAny<Meaning>()), Times.Once);
        }

        private void ShouldNotUpdate()
        {
            _repo.Verify(_ => _.Update(It.IsAny<Meaning>()), Times.Never);
        }

        [Fact]
        public void TryAdd_EntryExists_AddsProperly()
        {
            EntryExists();

            var result = service.Add(new());

            ShouldAdd();
            Assert.Empty(result);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void TryAdd_EntryDoesNotExist_ReturnsError()
        {
            EntryExists(false);

            var result = service.Add(new());

            ShouldNotAdd();
            Assert.Single(result);
            Assert.Equal("Entry does not exist.", result.First().Name);
        }

        [Fact]
        public void TryUpdate_MeaningNotFound_ReturnsError()
        {
            Exists(false);

            var result = service.Update(new());

            ShouldNotUpdate();
            Assert.Single(result);
            Assert.Equal("Entity does not exist.", result.First().Name);
        }

        [Fact]
        public void TryUpdate_MeaningExists_UpdatesProperly()
        {
            Exists();

            var result = service.Update(new());

            ShouldUpdate();
            Assert.Empty(result);
            Assert.True(result.IsValid);
        }
    }
}
