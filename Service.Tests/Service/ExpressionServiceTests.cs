using Data.Models;
using Moq;
using Service.Repository;
using System;
using System.Linq;
using Xunit;

namespace Service.Tests.Service
{
    public class ExpressionServiceTests : UowTestBase
    {
        IService<Expression> service;
        Mock<IExpressionRepository> _repo = new Mock<IExpressionRepository>();
        Mock<IDictionaryRepository> _dictRepo = new Mock<IDictionaryRepository>();
        Mock<IMeaningRepository> _meaningRepo = new Mock<IMeaningRepository>();

        public ExpressionServiceTests()
        {
            expRepo = _repo;
            dictRepo = _dictRepo;
            meaningRepo = _meaningRepo;
            service = new ExpressionService(this.uow.Object, VMoq.Instance<Expression>());
        }

        [Fact]
        public void TryAdd_MeaningExists_ShouldAdd()
        {
            var entity = new Expression
            {
                DictionaryIndex = null,
                MeaningID = 12,

            };

            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == entity.DictionaryIndex))).Returns(false);
            _meaningRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.MeaningID))).Returns(true);

            var result = service.TryAdd(entity);

            _repo.Verify(_ => _.Create(It.IsAny<Expression>()), Times.Once);
            Assert.Empty(result);

        }

        [Fact]
        public void TryAdd_MeaningDoesNotExist_ReturnsError()
        {
            var entity = new Expression
            {
                DictionaryIndex = null,
                MeaningID = 12,

            };

            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == entity.DictionaryIndex))).Returns(false);
            _meaningRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.MeaningID))).Returns(false);

            var result = service.TryAdd(entity);

            _repo.Verify(_ => _.Create(It.IsAny<Expression>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Meaning not found", result.First().Key);

        }

        [Fact]
        public void TryAdd_DictionaryDoesNotExist_ReturnsError()
        {
            var entity = new Expression
            {
                DictionaryIndex = 12,
                MeaningID = null,

            };

            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == entity.DictionaryIndex))).Returns(false);
            _meaningRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.MeaningID))).Returns(false);

            var result = service.TryAdd(entity);

            _repo.Verify(_ => _.Create(It.IsAny<Expression>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Dictionary not found", result.First().Key);
        }

        [Fact]
        public void TryAdd_BothFkeysDontExist_ReturnsErrors()
        {
            var entity = new Expression
            {
                DictionaryIndex = 12,
                MeaningID = 34,

            };

            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == entity.DictionaryIndex))).Returns(false);
            _meaningRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.MeaningID))).Returns(false);

            var result = service.TryAdd(entity);

            _repo.Verify(_ => _.Create(It.IsAny<Expression>()), Times.Never);
            Assert.Equal(2, result.Count);
            Assert.Equal("Meaning not found", result.First().Key);
            Assert.Equal("Dictionary not found", result.Take(2).Last().Key);
        }
        
        [Fact]
        public void TryUpdate_ExpressionExists_UpdatesProperly()
        {
            var entity = new Expression
            {
                DictionaryIndex = null,
                MeaningID = 34,
            };

            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == entity.DictionaryIndex))).Returns(false);
            _meaningRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.MeaningID))).Returns(true);
            _repo.Setup(_ => _.Exists(It.IsAny<System.Linq.Expressions.Expression<Func<Expression, bool>>>())).Returns(true);

            var result = service.TryUpdate(entity);

            _repo.Verify(_ => _.Update(It.IsAny<Expression>()), Times.Once);
            Assert.Empty(result);

        }

        [Fact]
        public void TryUpdate_ExpressionDoesNotExist_ReturnsError()
        {
            var entity = new Expression
            {
                DictionaryIndex = 12,
                MeaningID = 34,
            };

            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == entity.DictionaryIndex))).Returns(true);
            _meaningRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.MeaningID))).Returns(true);
            _repo.Setup(_ => _.Exists(It.IsAny<System.Linq.Expressions.Expression<Func<Expression, bool>>>())).Returns(false);

            var result = service.TryUpdate(entity);

            _repo.Verify(_ => _.Update(It.IsAny<Expression>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Entity does not exist", result.First().Key);
        }

        [Fact]
        public void TryUpdate_ExpressionExistsButAnotherErrorOccurs_ReturnsError()
        {
            var entity = new Expression
            {
                DictionaryIndex = 12,
                MeaningID = null,
            };

            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == entity.DictionaryIndex))).Returns(false);
            _meaningRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.MeaningID))).Returns(true);
            _repo.Setup(_ => _.Exists(It.IsAny<System.Linq.Expressions.Expression<Func<Expression, bool>>>())).Returns(true);

            var result = service.TryUpdate(entity);

            _repo.Verify(_ => _.Update(It.IsAny<Expression>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Dictionary not found", result.First().Key);
        }
    }

}
