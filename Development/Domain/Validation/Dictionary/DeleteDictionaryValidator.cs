using Domain.Dto;
using Domain.Models;
using Domain.Repository;
using FluentValidation;
using Msg = Domain.ValidationErrorMessages;

namespace Domain.Validation
{
    public class DeleteDictionaryValidator: BaseValidator<DeleteDictionaryCommand>
    {
        public DeleteDictionaryValidator(IUnitOfWork uow) : base(uow)
        {
            var repo = uow.Dictionaries;

            RuleFor(m => m.PrimaryKey)
                .Must(repo.ExistsByPrimaryKey)
                .WithMessage(Msg.EntityDoesNotExistByPrimaryKey<Dictionary>());
        }
    }
}