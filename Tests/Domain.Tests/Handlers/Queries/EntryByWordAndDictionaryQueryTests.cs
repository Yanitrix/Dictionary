using Domain.Dto;
using Domain.Mapper;
using Domain.Models;
using Domain.Queries;
using Domain.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.Tests.Handlers.Queries
{
    public class EntryByWordAndDictionaryQueryTests
    {
        private Mock<IMapper> mapper = new();
        private Mock<IEntryRepository> repo = new();
        private IQueryHandler<EntryByWordAndDictionaryQuery, IEnumerable<GetEntry>> sut;

        public EntryByWordAndDictionaryQueryTests()
        {
            mapper
                .Setup(_ => _.Map<Entry, GetEntry>(It.IsAny<Entry>()))
                .Returns<Entry>(e => new GetEntry
                {
                    ID = e.ID,
                });

            sut = new EntryByWordAndDictionaryQueryHandler(repo.Object, mapper.Object);
        }

        [Fact]
        public void BothArgumentsNull_ReturnsEmptyIEnumerable()
        {
            var query = new EntryByWordAndDictionaryQuery
            {
                DictionaryIndex = null,
                WordValue = null
            };

            var found = sut.Handle(query);

            Assert.NotNull(found);
            Assert.Empty(found);
        }

        [Fact]
        public void DictionaryIsNull_WordIsNotNull_CallsGetByWordMethod_ReturnsMappedDtos()
        {
            var query = new EntryByWordAndDictionaryQuery
            {
                DictionaryIndex = null,
                WordValue = "value"
            };

            repo.Setup(_ => _.GetByWord(query.WordValue, false))
                .Returns(new List<Entry>
                {
                    new()
                    {
                        ID = 1,
                    },

                    new()
                    {
                        ID = 5,
                    },
                    
                    new()
                    {
                        ID = 12,
                    }
                });

            var found = sut.Handle(query);

            //assert
            repo.Verify(_ => _.GetByWord(query.WordValue, false), Times.Once);
            Assert.NotNull(found);
            Assert.NotEmpty(found);
            Assert.Equal(3, found.Count());
            Assert.Equal(new int[] { 1, 5, 12 }, found.Select(x => x.ID));
        }

        [Fact]
        public void WordIsNull_DictionaryIsNotNull_CallsGetByDictionary_ReturnsMappedDtos()
        {
            var query = new EntryByWordAndDictionaryQuery
            {
                DictionaryIndex = 12,
                WordValue = null
            };

            repo.Setup(_ => _.GetByDictionary(query.DictionaryIndex.Value))
                .Returns(new List<Entry>
                {
                    new()
                    {
                        ID = 8,
                    },

                    new()
                    {
                        ID = 13,
                    },
                });

            //act
            var found = sut.Handle(query);

            //assert
            repo.Verify(_ => _.GetByDictionary(query.DictionaryIndex.Value), Times.Once);
            Assert.NotNull(found);
            Assert.NotEmpty(found);
            Assert.Equal(2, found.Count());
            Assert.Equal(new int[] { 8, 13 }, found.Select(x => x.ID));
        }

        [Fact]
        public void BothAgumentsArePresent_CallsGetByDictionaryAndWord_ReturnsMappedDtos()
        {
            var query = new EntryByWordAndDictionaryQuery
            {
                DictionaryIndex = 12,
                WordValue = "value"
            };

            repo.Setup(_ => _.GetByDictionaryAndWord(query.DictionaryIndex.Value, query.WordValue, false))
                .Returns(new List<Entry>
                {
                    new()
                    {
                        ID = 21,
                    },

                    new()
                    {
                        ID = 34,
                    },
                    
                    new()
                    {
                        ID = 55,
                    },
                    
                    new()
                    {
                        ID = 89,
                    },
                });

            //act
            var found = sut.Handle(query);
            
            //assert
            repo.Verify(_ => _.GetByDictionaryAndWord
            (
                query.DictionaryIndex.Value, query.WordValue, false),
                Times.Once
            );

            Assert.NotNull(found);
            Assert.NotEmpty(found);
            Assert.Equal(4, found.Count());
            Assert.Equal(new int[] { 21, 34, 55, 89 }, found.Select(x => x.ID));
        }

    }


}
