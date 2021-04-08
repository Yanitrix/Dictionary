using Domain.Dto;
using Domain.Models;
using Domain.Repository;
using FluentValidation;
using Msg = Domain.ValidationErrorMessages;

namespace Domain.Validation
{
    public class DeleteEntryValidator : BaseValidator<DeleteEntryCommand>
    {
        public DeleteEntryValidator(IUnitOfWork uow) : base(uow)
        {
            var repo = uow.Entries;

            RuleFor(m => m.PrimaryKey)
                .Must(repo.ExistsByPrimaryKey)
                .WithMessage(Msg.EntityDoesNotExistByPrimaryKey<Entry>());
        }
    }
}