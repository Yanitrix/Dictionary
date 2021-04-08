using System.Linq;
using FluentValidation;

namespace Domain.Commands
{
    public class ValidationCommandHandlerDecorator<T, R> : ICommandHandler<T, R> where T : ICommand
    {
        private readonly ICommandHandler<T, R> handler;
        private readonly AbstractValidator<T> validator;

        public ValidationCommandHandlerDecorator(ICommandHandler<T, R> handler, AbstractValidator<T> validator)
        {
            this.handler = handler;
            this.validator = validator;
        }

        public Response<R> Handle(T command)
        {
            var result = validator.Validate(command);
            
            if (result.IsValid)
                return handler.Handle(command);
            
            return Response<R>.ModelError(default, result.Errors.Select(x => x.ErrorMessage).ToList());
        }
    }
}