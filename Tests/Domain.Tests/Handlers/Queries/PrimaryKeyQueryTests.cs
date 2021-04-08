using Domain.Dto;
using Domain.Mapper;
using Domain.Models;
using Domain.Queries;
using Domain.Repository;
using Moq;
using Xunit;

namespace Domain.Tests.Handlers.Queries
{
    public class PrimaryKeyQueryTests
    {
        private Mock<IMapper> mapper = new();
        private Mock<IDictionaryRepository> dictionaries = new();
        private Mock<IEntryRepository> entries = new();
        private Mock<IFreeExpressionRepository> expressions = new();
        private Mock<ILanguageRepository> languages = new();
        private Mock<IMeaningRepository> meanings = new();
        private Mock<IWordRepository> words = new();
    
        [Fact]
        public void DictionaryByIndexQuery_NotFound_ReturnsNull()
        {
            //arrange
            var query = new DictionaryByIndexQuery(12);
            IQueryHandler<DictionaryByIndexQuery, GetDictionary> handler =
                new DictionaryByIndexQueryHandler(dictionaries.Object, mapper.Object);

            dictionaries.Setup(_ => _.GetByPrimaryKey(query.PrimaryKey)).Returns((Dictionary)null);
            //simplified mapping
            mapper.Setup(_ => _.Map<Dictionary, GetDictionary>(It.IsAny<Dictionary>())).Returns<Dictionary>(d => new GetDictionary
            {
                Index = d.Index,
                LanguageIn = d.LanguageInName,
                LanguageOut = d.LanguageOutName
            });

            //act
            var found = handler.Handle(query);

            //assert
            Assert.Null(found);
        }

        [Fact]
        public void DictionaryByIndexQuery_Found_MappedDto()
        {
            //arrange
            var query = new DictionaryByIndexQuery(12);
            IQueryHandler<DictionaryByIndexQuery, GetDictionary> handler
                = new DictionaryByIndexQueryHandler(dictionaries.Object, mapper.Object);

            dictionaries.Setup(_ => _.GetByPrimaryKey(query.PrimaryKey)).Returns(new Dictionary
            {
                Index = query.PrimaryKey,
                LanguageInName = "in",
                LanguageOutName = "out"
            });

            mapper.Setup(_ => _.Map<Dictionary, GetDictionary>(It.IsAny<Dictionary>())).Returns<Dictionary>(d => new GetDictionary
            {
                Index = d.Index,
                LanguageIn = d.LanguageInName,
                LanguageOut = d.LanguageOutName
            });

            //act
            var found = handler.Handle(query);

            //assert
            Assert.NotNull(found);
            Assert.Equal(query.PrimaryKey, found.Index);
            Assert.Equal("in", found.LanguageIn);
            Assert.Equal("out", found.LanguageOut);
        }

        [Fact]
        public void EntryByIdQuery_NotFound_ReturnsNull()
        {
            //arrange
            var query = new EntryByIdQuery(12);
            IQueryHandler<EntryByIdQuery, GetEntry> handler =
                new EntryByIdQueryHandler(entries.Object, mapper.Object);

            entries.Setup(_ => _.GetByPrimaryKey(query.PrimaryKey)).Returns((Entry)null);
            mapper.Setup(_ => _.Map<Entry, GetEntry>(It.IsAny<Entry>())).Returns<Entry>(d => new GetEntry
            {
                ID = d.ID,
            });

            //act
            var found = handler.Handle(query);

            //assert
            Assert.Null(found);
        }

        [Fact]
        public void EntryByIdQuery_Found_ReturnsMappedDto()
        {
            //arrange
            var query = new EntryByIdQuery(12);
            IQueryHandler<EntryByIdQuery, GetEntry> handler =
                new EntryByIdQueryHandler(entries.Object, mapper.Object);

            entries.Setup(_ => _.GetByPrimaryKey(query.PrimaryKey)).Returns(new Entry
            {
                ID = query.PrimaryKey,
                WordID = 10,
            });

            mapper.Setup(_ => _.Map<Entry, GetEntry>(It.IsAny<Entry>())).Returns<Entry>(d => new GetEntry
            {
                ID = d.ID,
            });

            //act
            var found = handler.Handle(query);

            //assert
            Assert.NotNull(found);
            Assert.Equal(query.PrimaryKey, found.ID);
        }

        [Fact]
        public void LanguageByNameQuery_NotFound_ReturnsNull()
        {
            var query = new LanguageByNameQuery("name");
            IQueryHandler<LanguageByNameQuery, GetLanguage> handler =
                new LanguageByNameQueryHandler(languages.Object, mapper.Object);

            languages.Setup(_ => _.GetByPrimaryKey(query.PrimaryKey)).Returns((Language)null);

            mapper.Setup(_ => _.Map<Language, GetLanguage>(It.IsAny<Language>())).Returns<Language>(d => new GetLanguage
            {
                Name = d.Name
            });

            var found = handler.Handle(query);

            Assert.Null(found);
        }

        [Fact]
        public void LanguageByNameQuery_Found_ReturnsMappedDto()
        {
            //arrange
            var query = new LanguageByNameQuery("name");
            IQueryHandler<LanguageByNameQuery, GetLanguage> handler =
                new LanguageByNameQueryHandler(languages.Object, mapper.Object);

            languages.Setup(_ => _.GetByPrimaryKey(query.PrimaryKey)).Returns(new Language
            {
                Name = query.PrimaryKey
            });

            mapper.Setup(_ => _.Map<Language, GetLanguage>(It.IsAny<Language>())).Returns<Language>(d => new GetLanguage
            {
                Name = d.Name
            });

            //act
            var found = handler.Handle(query);

            //assert
            Assert.NotNull(found);
            Assert.Equal(query.PrimaryKey, found.Name);
        }

        [Fact]
        public void MeaningByIdQuery_NotFound_ReturnsNull()
        {
            //arrange
            var query = new MeaningByIdQuery(12);
            IQueryHandler<MeaningByIdQuery, GetMeaning> handler
                = new MeaningByIdQueryHandler(meanings.Object, mapper.Object);

            meanings.Setup(_ => _.GetByPrimaryKey(query.PrimaryKey)).Returns((Meaning)null);
            mapper.Setup(_ => _.Map<Meaning, GetMeaning>(It.IsAny<Meaning>())).Returns<Meaning>(d => new GetMeaning
            {
                ID = d.ID,
                EntryID = d.EntryID
            });

            //act
            var found = handler.Handle(query);

            //assert
            Assert.Null(found);
        }

        [Fact]
        public void MeaningByIdQuery_Found_ReturnsMappedDto()
        {
            //arrange
            var query = new MeaningByIdQuery(12);
            IQueryHandler<MeaningByIdQuery, GetMeaning> handler =
                new MeaningByIdQueryHandler(meanings.Object, mapper.Object);

            meanings.Setup(_ => _.GetByPrimaryKey(query.PrimaryKey)).Returns(new Meaning
            {
                ID = query.PrimaryKey,
                EntryID = 10
            });

            mapper.Setup(_ => _.Map<Meaning, GetMeaning>(It.IsAny<Meaning>())).Returns<Meaning>(d => new GetMeaning
            {
                ID = d.ID,
                EntryID = d.EntryID
            });

            //act
            var found = handler.Handle(query);

            //assert
            Assert.NotNull(found);
            Assert.Equal(query.PrimaryKey, found.ID);
        }

        [Fact]
        public void FreeExpressionByIdQuery_NotFound_ReturnsNull()
        {
            //arrange
            var query = new FreeExpressionByIdQuery(12);
            IQueryHandler<FreeExpressionByIdQuery, GetFreeExpression> handler =
                new FreeExpressionByIdQueryHandler(expressions.Object, mapper.Object);

            expressions.Setup(_ => _.GetByPrimaryKey(query.PrimaryKey)).Returns((FreeExpression)null);
            mapper.Setup(_ => _.Map<FreeExpression, GetFreeExpression>(It.IsAny<FreeExpression>())).Returns<FreeExpression>(d => new GetFreeExpression
            {
                ID = d.ID,
                DictionaryIndex = d.DictionaryIndex
            });

            //act
            var found = handler.Handle(query);

            //assert
            Assert.Null(found);
        }

        [Fact]
        public void FreeExpressionByIdQuery_Found_ReturnsMappedDto()
        {
            //arrange
            var query = new FreeExpressionByIdQuery(12);
            IQueryHandler<FreeExpressionByIdQuery, GetFreeExpression> handler =
                new FreeExpressionByIdQueryHandler(expressions.Object, mapper.Object);

            expressions.Setup(_ => _.GetByPrimaryKey(query.PrimaryKey)).Returns(new FreeExpression
            {
                ID = query.PrimaryKey,
                DictionaryIndex = 10
            });

            mapper.Setup(_ => _.Map<FreeExpression, GetFreeExpression>(It.IsAny<FreeExpression>())).Returns<FreeExpression>(d => new GetFreeExpression
            {
                ID = d.ID,
                DictionaryIndex = d.DictionaryIndex
            });

            //act
            var found = handler.Handle(query);

            //assert
            Assert.NotNull(found);
            Assert.Equal(query.PrimaryKey, found.ID);
        }

        [Fact]
        public void WordByIdQuery_NotFound_ReturnsNull()
        {
            //arrange
            var query = new WordByIdQuery(12);
            IQueryHandler<WordByIdQuery, GetWord> handler =
                new WordByIdQueryHandler(words.Object, mapper.Object);

            words.Setup(_ => _.GetByPrimaryKey(query.PrimaryKey)).Returns((Word)null);
            mapper.Setup(_ => _.Map<Word, GetWord>(It.IsAny<Word>())).Returns<Word>(d => new GetWord
            {
                ID = d.ID,
                SourceLanguageName = d.SourceLanguageName,
                Value = d.Value
            });

            //act
            var found = handler.Handle(query);

            //assert
            Assert.Null(found);
        }

        [Fact]
        public void WordByIdQuery_Found_ReturnsMappedDto()
        {
            //arrange
            var query = new WordByIdQuery(12);
            IQueryHandler<WordByIdQuery, GetWord> handler
                = new WordByIdQueryHandler(words.Object, mapper.Object);

            words.Setup(_ => _.GetByPrimaryKey(query.PrimaryKey)).Returns(new Word
            {
                ID = query.PrimaryKey,
                SourceLanguageName = "name",
                Value = "value"
            });

            mapper.Setup(_ => _.Map<Word, GetWord>(It.IsAny<Word>())).Returns<Word>(d => new GetWord
            {
                ID = d.ID,
                SourceLanguageName = d.SourceLanguageName,
                Value = d.Value
            });

            //act
            var found = handler.Handle(query);

            //assert
            Assert.NotNull(found);
            Assert.Equal(query.PrimaryKey, found.ID);
        }
    }

}
