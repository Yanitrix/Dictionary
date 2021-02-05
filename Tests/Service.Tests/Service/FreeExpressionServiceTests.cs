using Data.Models;
using Moq;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Service.Tests.Service
{
    public class FreeExpressionServiceTests : UowTestBase
    {
        IService<FreeExpression> service;
        Mock<IFreeExpressionRepository> _repo = new Mock<IFreeExpressionRepository>();
        Mock<IDictionaryRepository> _dictRepo = new Mock<IDictionaryRepository>();


        public FreeExpressionServiceTests()
        {
            freeExpressionRepo = _repo;
            dictRepo = _dictRepo;
            service = new FreeExpressionService(this.uow.Object);
        }

        private FreeExpression entity { get => new FreeExpression { DictionaryIndex = 12 }; }

        [Fact]
        public void TryAdd_DictionaryExists_AddsProperly()
        {
            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == entity.DictionaryIndex))).Returns(true);

            var result = service.Add(entity);

            _repo.Verify(_ => _.Create(It.IsAny<FreeExpression>()), Times.Once);
            Assert.Empty(result);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void TryAdd_DictionaryDoesNotExist_ReturnsError()
        {
            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == entity.DictionaryIndex))).Returns(false);

            var result = service.Add(entity);

            _repo.Verify(_ => _.Create(It.IsAny<FreeExpression>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Dictionary does not exist.", result.First().Name);
        }


        [Fact]
        public void TryUpdate_FreeExpressionExistsButAnotherErrorOccurs_ReturnsError()
        {
            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == entity.DictionaryIndex))).Returns(false);
            _repo.Setup(_ => _.Exists(It.IsAny<System.Linq.Expressions.Expression<Func<FreeExpression, bool>>>())).Returns(true);

            var result = service.Update(entity);

            _repo.Verify(_ => _.Update(It.IsAny<FreeExpression>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Dictionary does not exist.", result.First().Name);
        }

        [Fact]
        public void TryUpdate_ExpressionExists_UpdatesProperly()
        {
            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == entity.DictionaryIndex))).Returns(true);
            _repo.Setup(_ => _.Exists(It.IsAny<System.Linq.Expressions.Expression<Func<FreeExpression, bool>>>())).Returns(true);

            var result = service.Update(entity);

            _repo.Verify(_ => _.Update(It.IsAny<FreeExpression>()), Times.Once);
            Assert.Empty(result);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void TryUpdate_ExpressionDoesNotExist_ReturnsError()
        {
            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == entity.DictionaryIndex))).Returns(true);
            _repo.Setup(_ => _.Exists(It.IsAny<System.Linq.Expressions.Expression<Func<FreeExpression, bool>>>())).Returns(false);

            var result = service.Update(entity);

            _repo.Verify(_ => _.Update(It.IsAny<FreeExpression>()), Times.Never);
            Assert.Single(result);
            Assert.False(result.IsValid);

        }
    }
}
