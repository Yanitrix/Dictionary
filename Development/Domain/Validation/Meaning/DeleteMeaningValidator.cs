using Domain.Dto;
using Domain.Models;
using Domain.Repository;
using FluentValidation;
using Msg = Domain.ValidationErrorMessages;

namespace Domain.Validation
{
    public class DeleteMeaningValidator : BaseValidator<DeleteMeaningCommand>
    {
        public DeleteMeaningValidator(IUnitOfWork uow) : base(uow)
        {
            var repo = uow.Meanings;

            RuleFor(x => x.PrimaryKey)
                .Must(repo.ExistsByPrimaryKey)
                .WithMessage(Msg.EntityDoesNotExistByPrimaryKey<Meaning>());
        }
    }
}