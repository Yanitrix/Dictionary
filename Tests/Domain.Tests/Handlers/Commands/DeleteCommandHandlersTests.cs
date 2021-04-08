using Domain.Commands;
using Domain.Dto;
using Domain.Models;
using Domain.Repository;
using Moq;
using System;
using Xunit;

namespace Domain.Tests.Handlers.Commands
{
    public class DeleteCommandHandlersTests
    {
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
        public void TestDeleteDictionaryCommandHandler()
        {
            var command = new DeleteDictionaryCommand();
            var found = new Dictionary();

            ICommandHandler<DeleteDictionaryCommand, Dictionary> handler = new DeleteDictionaryCommandHandler(dictionaries.Object);
            dictionaries.Setup(_ => _.GetByPrimaryKey(It.IsAny<int>())).Returns(found);

            var result = handler.Handle(command);

            dictionaries.Verify(_ => _.GetByPrimaryKey(It.IsAny<int>()));
            dictionaries.Verify(_ => _.Delete(found));

            Assert.True(result.IsSuccessful);
            Assert.IsType<Dictionary>(result.Entity);
        }

        [Fact]
        public void TestDeleteEntryCommandHandler()
        {
            var command = new DeleteEntryCommand();
            var found = new Entry();

            ICommandHandler<DeleteEntryCommand, Entry> handler = new DeleteEntryCommandHandler(entries.Object);
            entries.Setup(_ => _.GetByPrimaryKey(It.IsAny<int>())).Returns(found);

            var result = handler.Handle(command);

            entries.Verify(_ => _.GetByPrimaryKey(It.IsAny<int>()));
            entries.Verify(_ => _.Delete(found));

            Assert.True(result.IsSuccessful);
            Assert.IsType<Entry>(result.Entity);
        }

        [Fact]
        public void TestDeleteMeaningCommandHandler()
        {
            var command = new DeleteMeaningCommand();
            var found = new Meaning();

            ICommandHandler<DeleteMeaningCommand, Meaning> handler = new DeleteMeaningCommandHandler(meanings.Object);
            meanings.Setup(_ => _.GetByPrimaryKey(It.IsAny<int>())).Returns(found);

            var result = handler.Handle(command);

            meanings.Verify(_ => _.GetByPrimaryKey(It.IsAny<int>()));
            meanings.Verify(_ => _.Delete(found));

            Assert.True(result.IsSuccessful);
            Assert.IsType<Meaning>(result.Entity);
        }

        [Fact]
        public void TestDeleteLanguageCommandHandler()
        {
            var command = new DeleteLanguageCommand();
            var found = new Language();

            ICommandHandler<DeleteLanguageCommand, Language> handler = new DeleteLanguageCommandHandler(languages.Object);
            languages.Setup(_ => _.GetByPrimaryKey(It.IsAny<String>())).Returns(found);

            var result = handler.Handle(command);

            languages.Verify(_ => _.GetByPrimaryKey(It.IsAny<String>()));
            languages.Verify(_ => _.Delete(found));

            Assert.True(result.IsSuccessful);
            Assert.IsType<Language>(result.Entity);
        }

        [Fact]
        public void DeleteFreeExpressionCommandHandler()
        {
            var command = new DeleteFreeExpressionCommand();
            var found = new FreeExpression();

            ICommandHandler<DeleteFreeExpressionCommand, FreeExpression> handler = new DeleteFreeExpressionCommandHandler(expressions.Object);
            expressions.Setup(_ => _.GetByPrimaryKey(It.IsAny<int>())).Returns(found);

            var result = handler.Handle(command);

            expressions.Verify(_ => _.GetByPrimaryKey(It.IsAny<int>()));
            expressions.Verify(_ => _.Delete(found));

            Assert.True(result.IsSuccessful);
            Assert.IsType<FreeExpression>(result.Entity);
        }

        [Fact]
        public void DeleteWordCommandHandler()
        {
            var command = new DeleteWordCommand();
            var found = new Word();

            ICommandHandler<DeleteWordCommand, Word> handler = new DeleteWordCommandHandler(words.Object);
            words.Setup(_ => _.GetByPrimaryKey(It.IsAny<int>())).Returns(found);

            var result = handler.Handle(command);

            words.Verify(_ => _.GetByPrimaryKey(It.IsAny<int>()));
            words.Verify(_ => _.Delete(found));

            Assert.True(result.IsSuccessful);
            Assert.IsType<Word>(result.Entity);
        }
    }
}
