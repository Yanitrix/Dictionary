using Domain.Dto;
using Domain.Models;
using Domain.Repository;
using FluentValidation;
using Msg = Domain.ValidationErrorMessages;

namespace Domain.Validation
{
    public class DeleteWordValidator : BaseValidator<DeleteWordCommand>
    {
        public DeleteWordValidator(IUnitOfWork uow) : base(uow)
        {
            var repo = uow.Words;

            RuleFor(x => x.PrimaryKey)
                .Must(repo.ExistsByPrimaryKey)
                .WithMessage(Msg.EntityDoesNotExistByPrimaryKey<Word>());
        }
    }
}