using Domain.Dto;
using FluentValidation;
using System;
using Domain.Models;
using Domain.Repository;
using Msg = Domain.ValidationErrorMessages;

namespace Domain.Validation
{
    public class UpdateMeaningValidator : BaseValidator<UpdateMeaningCommand>
    {
        public UpdateMeaningValidator(IUnitOfWork uow):base(uow)
        {
            SetPropertyRules();
            SetRelationRules();
        }

        private void SetPropertyRules()
        {
            RuleFor(m => m.Value).Cascade(CascadeMode.Stop).NotEmpty().NoDigits(); //cannot be empty, if user wants to use only examples then they're encouraged to use FreeExpression instead
            RuleFor(m => m.Notes).Cascade(CascadeMode.Stop).NotEmpty().NoDigits().When(m => !String.IsNullOrWhiteSpace(m.Notes));
            RuleForEach(m => m.Examples).SetValidator(new ExampleDtoValidator()).When(m => m.Examples.Count > 0);
        }

        private void SetRelationRules()
        {
            RuleFor(m => m.ID)
                .Must(uow.Meanings.ExistsByPrimaryKey)
                .WithMessage(Msg.EntityDoesNotExistByPrimaryKey<Meaning>());
        }
    }
}
