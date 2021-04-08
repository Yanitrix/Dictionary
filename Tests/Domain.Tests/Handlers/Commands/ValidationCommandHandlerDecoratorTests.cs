using Domain.Commands;
using Domain.Dto;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Domain.Tests.Handlers.Commands
{
    class PositiveValidatorMock<T> : AbstractValidator<T>
    {
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            return new(new List<ValidationFailure>());
        }
    }

    class NegativeValidatorMock<T> : AbstractValidator<T>
    {
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            return new(new List<ValidationFailure>
                {
                    new("", "")
                });
        }
    }

    //gonna test all commands? idk, there are no edge cases
    public class ValidationCommandHandlerDecoratorTests
    {
        [Fact]
        public void CreateDictionaryCommandHandler_NoErrors_CallsInner()
        {
            //arrange
            var command = new CreateDictionaryCommand();
            var validator = new PositiveValidatorMock<CreateDictionaryCommand>();
            var innerMock = new Mock<ICommandHandler<CreateDictionaryCommand, GetDictionary>>();
            innerMock.Setup(_ => _.Handle(command)).Returns(Response<GetDictionary>.Ok(new()));
            var inner = innerMock.Object;

            ICommandHandler<CreateDictionaryCommand, GetDictionary> handler =
                new ValidationCommandHandlerDecorator<CreateDictionaryCommand, GetDictionary>(inner, validator);

            //act
            var response = handler.Handle(command);

            //assert
            innerMock.Verify(_ => _.Handle(command), Times.Once);
            Assert.True(response.IsSuccessful);
        }

        [Fact]
        public void CreateDictionaryCommandHandler_ValidationFailed_ReturnsFailedResponse()
        {
            //arrange
            var command = new CreateDictionaryCommand();
            var validator = new NegativeValidatorMock<CreateDictionaryCommand>();
            var innerMock = new Mock<ICommandHandler<CreateDictionaryCommand, GetDictionary>>();
            innerMock.Setup(_ => _.Handle(command)).Returns(Response<GetDictionary>.Ok(new()));
            var inner = innerMock.Object;

            ICommandHandler<CreateDictionaryCommand, GetDictionary> handler =
                new ValidationCommandHandlerDecorator<CreateDictionaryCommand, GetDictionary>(inner, validator);

            //act
            var response = handler.Handle(command);

            //assert
            innerMock.Verify(_ => _.Handle(command), Times.Never);
            Assert.False(response.IsSuccessful);
        }

        [Fact]
        public void UpdateWordCommandHandler_ValidationSucceeded_CallsInner()
        {
            //arrange
            var command = new UpdateWordCommand();
            var validator = new PositiveValidatorMock<UpdateWordCommand>();
            var innerMock = new Mock<ICommandHandler<UpdateWordCommand, GetWord>>();
            innerMock.Setup(_ => _.Handle(command)).Returns(Response<GetWord>.Ok(new()));
            var inner = innerMock.Object;

            ICommandHandler<UpdateWordCommand, GetWord> handler =
                new ValidationCommandHandlerDecorator<UpdateWordCommand, GetWord>(inner, validator);

            //act
            var response = handler.Handle(command);

            //assert
            innerMock.Verify(_ => _.Handle(command), Times.Once);
            Assert.True(response.IsSuccessful);
        }

        [Fact]
        public void UpdateWordCommandHandler_ValidationFailed_ReturnsFailedResponse()
        {
            //arrange
            var command = new UpdateWordCommand();
            var validator = new NegativeValidatorMock<UpdateWordCommand>();
            var innerMock = new Mock<ICommandHandler<UpdateWordCommand, GetWord>>();
            innerMock.Setup(_ => _.Handle(command)).Returns(Response<GetWord>.Ok(new()));
            var inner = innerMock.Object;

            ICommandHandler<UpdateWordCommand, GetWord> handler =
                new ValidationCommandHandlerDecorator<UpdateWordCommand, GetWord>(inner, validator);

            //act
            var response = handler.Handle(command);

            //assert
            innerMock.Verify(_ => _.Handle(command), Times.Never);
            Assert.False(response.IsSuccessful);
        }

        [Fact]
        public void DeleteEntryCommandHandler_ValidationSucceeded_CallsInner()
        {
            //arrange
            var command = new DeleteEntryCommand();
            var validator = new PositiveValidatorMock<DeleteEntryCommand>();
            var innerMock = new Mock<ICommandHandler<DeleteEntryCommand, Entry>>();
            innerMock.Setup(_ => _.Handle(command)).Returns(Response<Entry>.Ok(new()));
            var inner = innerMock.Object;

            ICommandHandler<DeleteEntryCommand, Entry> handler =
                new ValidationCommandHandlerDecorator<DeleteEntryCommand, Entry>(inner, validator);

            //act
            var response = handler.Handle(command);

            //assert
            innerMock.Verify(_ => _.Handle(command), Times.Once);
            Assert.True(response.IsSuccessful);
        }

        [Fact]
        public void DeleteEntryCommandHandler_ValidationFailed_ReturnsFailedResponse()
        {
            //arrange
            var command = new DeleteEntryCommand();
            var validator = new NegativeValidatorMock<DeleteEntryCommand>();
            var innerMock = new Mock<ICommandHandler<DeleteEntryCommand, Entry>>();
            innerMock.Setup(_ => _.Handle(command)).Returns(Response<Entry>.Ok(new()));
            var inner = innerMock.Object;

            ICommandHandler<DeleteEntryCommand, Entry> handler =
                new ValidationCommandHandlerDecorator<DeleteEntryCommand, Entry>(inner, validator);

            //act
            var response = handler.Handle(command);

            //assert
            innerMock.Verify(_ => _.Handle(command), Times.Never);
            Assert.False(response.IsSuccessful);
        }
    }
}
