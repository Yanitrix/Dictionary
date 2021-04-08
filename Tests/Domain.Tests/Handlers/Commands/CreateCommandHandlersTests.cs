using Domain.Commands;
using Domain.Dto;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;
using Moq;
using Xunit;

namespace Domain.Tests.Handlers.Commands
{
    public class CreateCommandHandlersTests
    {
        private readonly Mock<IMapper> mapper = new();
        private readonly Mock<IDictionaryRepository> dictionaries = new();
        private readonly Mock<IEntryRepository> entries = new();
        private readonly Mock<IFreeExpressionRepository> expressions = new();
        private readonly Mock<IMeaningRepository> meanings = new();
        private readonly Mock<ILanguageRepository> languages = new();
        private readonly Mock<IWordRepository> words = new();

        /// <summary>
        /// Every command does the same just with different types. I test them all because I want to have all
        /// possible use cases of the generic class tested.
        /// Every command should just call mapper and given respository, then return the response.
        /// </summary>
        [Fact]
        public void TestCreateDictionaryCommandHandler()
        {
            var command = new CreateDictionaryCommand
            {
                LanguageIn = "in",
                LanguageOut = "out"
            };

            ICommandHandler<CreateDictionaryCommand, GetDictionary> handler = new CreateDictionaryCommandHandler(mapper.Object, dictionaries.Object);

            mapper.Setup(_ => _.Map<CreateDictionaryCommand, Dictionary>(command)).Returns(new Dictionary());
            mapper.Setup(_ => _.Map<Dictionary, GetDictionary>(It.IsAny<Dictionary>())).Returns(new GetDictionary());

            //act
            var response = handler.Handle(command);

            //assert
            mapper.Verify(_ => _.Map<CreateDictionaryCommand, Dictionary>(It.IsAny<CreateDictionaryCommand>()));
            mapper.Verify(_ => _.Map<Dictionary, GetDictionary>(It.IsAny<Dictionary>()));
            dictionaries.Verify(_ => _.Create(It.IsAny<Dictionary>()));

            Assert.True(response.IsSuccessful);
            Assert.IsType<GetDictionary>(response.Entity);
        }

        [Fact]
        public void TestCreateEntryCommandHandler()
        {
            var command = new CreateEntryCommand
            {
                DictionaryIndex = 1,
                WordID = 1,
            };

            ICommandHandler<CreateEntryCommand, GetEntry> handler = new CreateEntryCommandHandler(mapper.Object, entries.Object);

            mapper.Setup(_ => _.Map<CreateEntryCommand, Entry>(It.IsAny<CreateEntryCommand>())).Returns(new Entry());
            mapper.Setup(_ => _.Map<Entry, GetEntry>(It.IsAny<Entry>())).Returns(new GetEntry());

            //act
            var response = handler.Handle(command);

            //assert
            mapper.Verify(_ => _.Map<CreateEntryCommand, Entry>(It.IsAny<CreateEntryCommand>()));
            mapper.Verify(_ => _.Map<Entry, GetEntry>(It.IsAny<Entry>()));
            entries.Verify(_ => _.Create(It.IsAny<Entry>()));

            Assert.True(response.IsSuccessful);
            Assert.IsType<GetEntry>(response.Entity);
        }

        [Fact]
        public void TestCreateFreeExpressionCommandHandler()
        {
            var command = new CreateFreeExpressionCommand();

            ICommandHandler<CreateFreeExpressionCommand, GetFreeExpression> handler = new CreateFreeExpressionCommandHandler(mapper.Object, expressions.Object);

            mapper.Setup(_ => _.Map<CreateFreeExpressionCommand, FreeExpression>(It.IsAny<CreateFreeExpressionCommand>())).Returns(new FreeExpression());
            mapper.Setup(_ => _.Map<FreeExpression, GetFreeExpression>(It.IsAny<FreeExpression>())).Returns(new GetFreeExpression());

            //act
            var response = handler.Handle(command);

            //assert
            mapper.Verify(_ => _.Map<CreateFreeExpressionCommand, FreeExpression>(It.IsAny<CreateFreeExpressionCommand>()));
            mapper.Verify(_ => _.Map<FreeExpression, GetFreeExpression>(It.IsAny<FreeExpression>()));
            expressions.Verify(_ => _.Create(It.IsAny<FreeExpression>()));

            Assert.True(response.IsSuccessful);
            Assert.IsType<GetFreeExpression>(response.Entity);
        }

        [Fact]
        public void TestCreateLanguageCommandHandler()
        {
            var command = new CreateLanguageCommand();

            ICommandHandler<CreateLanguageCommand, GetLanguage> handler = new CreateLanguageCommandHandler(mapper.Object, languages.Object);

            mapper.Setup(_ => _.Map<CreateLanguageCommand, Language>(It.IsAny<CreateLanguageCommand>())).Returns(new Language());
            mapper.Setup(_ => _.Map<Language, GetLanguage>(It.IsAny<Language>())).Returns(new GetLanguage());

            //act
            var response = handler.Handle(command);

            //assert
            mapper.Verify(_ => _.Map<CreateLanguageCommand, Language>(It.IsAny<CreateLanguageCommand>()));
            mapper.Verify(_ => _.Map<Language, GetLanguage>(It.IsAny<Language>()));
            languages.Verify(_ => _.Create(It.IsAny<Language>()));

            Assert.True(response.IsSuccessful);
            Assert.IsType<GetLanguage>(response.Entity);
        }

        [Fact]
        public void TestCreateMeaningCommandHandler()
        {
            var command = new CreateMeaningCommand();

            ICommandHandler<CreateMeaningCommand, GetMeaning> handler = new CreateMeaningCommandHandler(mapper.Object, meanings.Object);

            mapper.Setup(_ => _.Map<CreateMeaningCommand, Meaning>(It.IsAny<CreateMeaningCommand>())).Returns(new Meaning());
            mapper.Setup(_ => _.Map<Meaning, GetMeaning>(It.IsAny<Meaning>())).Returns(new GetMeaning());

            //act
            var response = handler.Handle(command);

            //assert
            mapper.Verify(_ => _.Map<CreateMeaningCommand, Meaning>(It.IsAny<CreateMeaningCommand>()));
            mapper.Verify(_ => _.Map<Meaning, GetMeaning>(It.IsAny<Meaning>()));
            meanings.Verify(_ => _.Create(It.IsAny<Meaning>()));

            Assert.True(response.IsSuccessful);
            Assert.IsType<GetMeaning>(response.Entity);
        }

        [Fact]
        public void TestCreateWordCommandHandler()
        {
            var command = new CreateWordCommand();

            ICommandHandler<CreateWordCommand, GetWord> handler = new CreateWordCommandHandler(mapper.Object, words.Object);

            mapper.Setup(_ => _.Map<CreateWordCommand, Word>(It.IsAny<CreateWordCommand>())).Returns(new Word());
            mapper.Setup(_ => _.Map<Word, GetWord>(It.IsAny<Word>())).Returns(new GetWord());

            //act
            var response = handler.Handle(command);

            //assert
            mapper.Verify(_ => _.Map<CreateWordCommand, Word>(It.IsAny<CreateWordCommand>()));
            mapper.Verify(_ => _.Map<Word, GetWord>(It.IsAny<Word>()));
            words.Verify(_ => _.Create(It.IsAny<Word>()));

            Assert.True(response.IsSuccessful);
            Assert.IsType<GetWord>(response.Entity);
        }
    }
}
