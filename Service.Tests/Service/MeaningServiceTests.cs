using Service.Repository;
using Data.Models;
using Moq;
using System.Linq;
using Xunit;

namespace Service.Tests.Service
{
    public class MeaningServiceTests
    {
        IService<Meaning> service;
        Mock<IMeaningRepository> repo = new Mock<IMeaningRepository>();
        Mock<IEntryRepository> entryRepo = new Mock<IEntryRepository>();

        public MeaningServiceTests()
        {
            service = new MeaningService(repo.Object, entryRepo.Object, VMoq.Instance<Meaning>());
        }

        [Fact]
        public void TryAdd_EntryExists_AddsProperly()
        {
            entryRepo.Setup(_ => _.ExistsByID(It.IsAny<int>())).Returns(true);

            var result = service.TryAdd(new Meaning());

            repo.Verify(_ => _.Create(It.IsAny<Meaning>()), Times.Once);
            Assert.Empty(result);
            Assert.True(result.IsValid);

        }

        [Fact]
        public void TryAdd_EntryDoesNotExist_ReturnsError()
        {
            entryRepo.Setup(_ => _.ExistsByID(It.IsAny<int>())).Returns(false);

            var result = service.TryAdd(new Meaning());

            repo.Verify(_ => _.Create(It.IsAny<Meaning>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Entry not found", result.First().Key);

        }

        [Fact]
        public void TryUpdate_MeaningNotFound_ReturnsError()
        {
            var entity = new Meaning
            {
                EntryID = 1,
                ID = 12
            };
            
            repo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.ID))).Returns(false);

            var result = service.TryUpdate(new Meaning());

            repo.Verify(_ => _.Update(It.IsAny<Meaning>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Entity does not exist", result.First().Key);

        }

        [Fact]
        public void TryUpdate_MeaningExists_UpdatesProperly()
        {
            var entity = new Meaning
            {
                EntryID = 1,
                ID = 12
            };

            repo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.ID))).Returns(true);
            entryRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.EntryID))).Returns(true);

            var result = service.TryUpdate(entity);

            repo.Verify(_ => _.Update(It.IsAny<Meaning>()), Times.Once);
            Assert.Empty(result);
            Assert.True(result.IsValid);

        }

    }
}
