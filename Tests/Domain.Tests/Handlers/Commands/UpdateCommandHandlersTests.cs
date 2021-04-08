using Domain.Commands;
using Domain.Dto;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;
using Moq;
using Xunit;

namespace Domain.Tests.Handlers.Commands
{
    public class UpdateCommandHandlersTests
    {
        private readonly Mock<IMapper> mapper = new();
        private readonly Mock<IEntryRepository> entries = new();
        private readonly Mock<IFreeExpressionRepository> expressions = new();
        private readonly Mock<IMeaningRepository> meanings = new();
        private readonly Mock<IWordRepository> words = new();

        /// <summary>
        /// Every command does the same just with different types. I test them all because I want to have all
        /// possible use cases of the generic class tested.
        /// Every command should just call mapper and given respository, then return the response.
        /// </summary>

        [Fact]
        public void TestUpdateEntryCommandHandler()
        {
            var command = new UpdateEntryCommand();

            ICommandHandler<UpdateEntryCommand, GetEntry> handler = new UpdateEntryCommandHandler(mapper.Object, entries.Object);

            mapper.Setup(_ => _.Map<UpdateEntryCommand, Entry>(It.IsAny<UpdateEntryCommand>())).Returns(new Entry());
            mapper.Setup(_ => _.Map<Entry, GetEntry>(It.IsAny<Entry>())).Returns(new GetEntry());

            //act
            var response = handler.Handle(command);

            //assert
            mapper.Verify(_ => _.Map<UpdateEntryCommand, Entry>(It.IsAny<UpdateEntryCommand>()));
            mapper.Verify(_ => _.Map<Entry, GetEntry>(It.IsAny<Entry>()));
            entries.Verify(_ => _.Update(It.IsAny<Entry>()));

            Assert.True(response.IsSuccessful);
            Assert.IsType<GetEntry>(response.Entity);
        }

        [Fact]
        public void TestUpdateFreeExpressionCommandHandler()
        {
            var command = new UpdateFreeExpressionCommand();

            ICommandHandler<UpdateFreeExpressionCommand, GetFreeExpression> handler = new UpdateFreeExpressionCommandHandler(mapper.Object, expressions.Object);

            mapper.Setup(_ => _.Map<UpdateFreeExpressionCommand, FreeExpression>(It.IsAny<UpdateFreeExpressionCommand>())).Returns(new FreeExpression());
            mapper.Setup(_ => _.Map<FreeExpression, GetFreeExpression>(It.IsAny<FreeExpression>())).Returns(new GetFreeExpression());

            //act
            var response = handler.Handle(command);

            //assert
            mapper.Verify(_ => _.Map<UpdateFreeExpressionCommand, FreeExpression>(It.IsAny<UpdateFreeExpressionCommand>()));
            mapper.Verify(_ => _.Map<FreeExpression, GetFreeExpression>(It.IsAny<FreeExpression>()));
            expressions.Verify(_ => _.Update(It.IsAny<FreeExpression>()));

            Assert.True(response.IsSuccessful);
            Assert.IsType<GetFreeExpression>(response.Entity);
        }

        [Fact]
        public void TestUpdateMeaningCommandHandler()
        {
            var command = new UpdateMeaningCommand();

            ICommandHandler<UpdateMeaningCommand, GetMeaning> handler = new UpdateMeaningCommandHandler(mapper.Object, meanings.Object);

            mapper.Setup(_ => _.Map<UpdateMeaningCommand, Meaning>(It.IsAny<UpdateMeaningCommand>())).Returns(new Meaning());
            mapper.Setup(_ => _.Map<Meaning, GetMeaning>(It.IsAny<Meaning>())).Returns(new GetMeaning());

            //act
            var response = handler.Handle(command);

            //assert
            mapper.Verify(_ => _.Map<UpdateMeaningCommand, Meaning>(It.IsAny<UpdateMeaningCommand>()));
            mapper.Verify(_ => _.Map<Meaning, GetMeaning>(It.IsAny<Meaning>()));
            meanings.Verify(_ => _.Update(It.IsAny<Meaning>()));

            Assert.True(response.IsSuccessful);
            Assert.IsType<GetMeaning>(response.Entity);
        }

        [Fact]
        public void TestUpdateWordCommandHandler()
        {
            var command = new UpdateWordCommand();

            ICommandHandler<UpdateWordCommand, GetWord> handler = new UpdateWordCommandHandler(mapper.Object, words.Object);

            mapper.Setup(_ => _.Map<UpdateWordCommand, Word>(It.IsAny<UpdateWordCommand>())).Returns(new Word());
            mapper.Setup(_ => _.Map<Word, GetWord>(It.IsAny<Word>())).Returns(new GetWord());

            //act
            var response = handler.Handle(command);

            //assert
            mapper.Verify(_ => _.Map<UpdateWordCommand, Word>(It.IsAny<UpdateWordCommand>()));
            mapper.Verify(_ => _.Map<Word, GetWord>(It.IsAny<Word>()));
            words.Verify(_ => _.Update(It.IsAny<Word>()));

            Assert.True(response.IsSuccessful);
            Assert.IsType<GetWord>(response.Entity);
        }
    }
}
