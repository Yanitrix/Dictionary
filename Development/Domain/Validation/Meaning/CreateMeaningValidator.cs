using Domain.Dto;
using Domain.Messages;
using FluentValidation;
using System;
using Domain.Models;
using Domain.Repository;
using Msg = Domain.ValidationErrorMessages;

namespace Domain.Validation
{
    public class CreateMeaningValidator : BaseValidator<CreateMeaningCommand>
    {
        public CreateMeaningValidator(IUnitOfWork uow):base(uow)
        {
            SetPropertyRules();
            SetRelationRules();
        }

        private void SetPropertyRules()
        {
            RuleFor(m => m.Value).Cascade(CascadeMode.Stop).NotEmpty().NoDigits();
            RuleFor(m => m.Notes).Cascade(CascadeMode.Stop).NotEmpty().NoDigits().When(m => !String.IsNullOrWhiteSpace(m.Notes));
            RuleFor(m => m.EntryID).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
            RuleForEach(m => m.Examples).SetValidator(new ExampleDtoValidator()).When(m => m.Examples.Count > 0);
        }

        private void SetRelationRules()
        {
            RuleFor(m => m.EntryID)
                .Must(uow.Entries.ExistsByPrimaryKey)
                .WithMessage(id => Msg.EntityDoesNotExistByForeignKey<Meaning, Entry>(e => e.ID, id));
        }
    }
}
