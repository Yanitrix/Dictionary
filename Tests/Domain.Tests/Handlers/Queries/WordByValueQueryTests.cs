using Domain.Dto;
using Domain.Mapper;
using Domain.Models;
using Domain.Queries;
using Domain.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Tests.Handlers.Queries
{
    public class WordByValueQueryTests
    {
        private readonly IQueryHandler<WordByValueQuery, IEnumerable<GetWord>> sut;
        private readonly Mock<IMapper> mapper = new();
        private readonly Mock<IWordRepository> repo = new();

        public WordByValueQueryTests()
        {
            sut = new WordByValueQueryHandler(repo.Object, mapper.Object);

            mapper.Setup(_ => _.Map<Word, GetWord>(It.IsAny<Word>())).Returns<Word>(w => new GetWord
            {
                ID = w.ID,
                SourceLanguageName = w.SourceLanguageName,
                Value = w.Value
            }); //simplified mapping
        }

        [Fact]
        public void HandleQuery_NothingFound_ReturnsEmpty()
        {
            var query = new WordByValueQuery("string");
            repo.Setup(_ => _.GetByValue(It.IsAny<String>(), false)).Returns(Array.Empty<Word>());

            var result = sut.Handle(query);

            Assert.Empty(result);
        }

        [Fact]
        public void HandlerQuery_EntitiesFound_ReturnsCollection()
        {
            var query = new WordByValueQuery("string");
            repo.Setup(_ => _.GetByValue(It.IsAny<String>(), false)).Returns(new List<Word>()
            {
                new()
                {
                    ID = 1,
                    SourceLanguageName = "1",
                    Value = "1"
                },

                new()
                {
                    ID = 1,
                    SourceLanguageName = "1",
                    Value = "1"
                },

                new()
                {
                    ID = 1,
                    SourceLanguageName = "1",
                    Value = "1"
                },
            });

            var result = sut.Handle(query);

            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count());
        }
    }
}
