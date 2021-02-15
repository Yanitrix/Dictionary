using Data.Dto;
using Data.Messages;
using FluentValidation;
using System;

namespace Data.Validation
{
    public class CreateMeaningValidator : AbstractValidator<CreateMeaning>
    {
        public CreateMeaningValidator()
        {
            RuleFor(m => m.Value).Cascade(CascadeMode.Stop).NotEmpty().NoDigits(); //cannot be empty, if user wants to use only examples then they're encouraged to use FreeExpression instead
            RuleFor(m => m.Notes).Cascade(CascadeMode.Stop).NotEmpty().NoDigits().When(m => !String.IsNullOrWhiteSpace(m.Notes));
            RuleFor(m => m.EntryID).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
            RuleForEach(m => m.Examples).SetValidator(new ExampleDtoValidator()).When(m => m.Examples.Count > 0);
        }
    }
}
