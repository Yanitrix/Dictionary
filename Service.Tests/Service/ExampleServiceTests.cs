using Data.Models;
using Moq;
using Service.Repository;
using System;
using System.Linq;
using Xunit;

namespace Service.Tests.Service
{
    public class ExampleServiceTests : UowTestBase
    {
        IService<Example> service;
        Mock<IExampleRepository> _repo = new Mock<IExampleRepository>();
        Mock<IMeaningRepository> _meaningRepo = new Mock<IMeaningRepository>();

        public ExampleServiceTests()
        {
            exampleRepo = _repo;
            meaningRepo = _meaningRepo;
            service = new ExampleService(this.uow.Object);
        }

        [Fact]
        public void TryAdd_MeaningExists_ShouldAdd()
        {
            var entity = new Example
            {
                MeaningID = 12,

            };

            _meaningRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.MeaningID))).Returns(true);

            var result = service.Add(entity);

            _repo.Verify(_ => _.Create(It.IsAny<Example>()), Times.Once);
            Assert.Empty(result);

        }

        [Fact]
        public void TryAdd_MeaningDoesNotExist_ReturnsError()
        {
            var entity = new Example
            {
                MeaningID = 12,
            };

            _meaningRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.MeaningID))).Returns(false);

            var result = service.Add(entity);

            _repo.Verify(_ => _.Create(It.IsAny<Example>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Meaning not found", result.First().Key);

        }

        [Fact]
        public void TryUpdate_ExpressionExists_UpdatesProperly()
        {
            var entity = new Example
            {
                MeaningID = 34,
            };

            _meaningRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.MeaningID))).Returns(true);
            _repo.Setup(_ => _.Exists(It.IsAny<System.Linq.Expressions.Expression<Func<Example, bool>>>())).Returns(true);

            var result = service.Update(entity);

            _repo.Verify(_ => _.Update(It.IsAny<Example>()), Times.Once);
            Assert.Empty(result);

        }

        [Fact]
        public void TryUpdate_ExpressionDoesNotExist_ReturnsError()
        {
            var entity = new Example
            {
                MeaningID = 34,
            };

            _meaningRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.MeaningID))).Returns(true);
            _repo.Setup(_ => _.Exists(It.IsAny<System.Linq.Expressions.Expression<Func<Example, bool>>>())).Returns(false);

            var result = service.Update(entity);

            _repo.Verify(_ => _.Update(It.IsAny<Example>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Entity does not exist", result.First().Key);
        }

        }
    }

