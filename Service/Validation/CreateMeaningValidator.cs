using Commons;
using Data.Dto;
using FluentValidation;
using System;

namespace Service.Validation
{
    class CreateMeaningValidator : AbstractValidator<CreateMeaning>
    {
        public CreateMeaningValidator()
        {
            RuleFor(m => m.Value).NotEmpty().NoDigits(); //cannot be empty, if user wants to use only examples then they're encouraged to use FreeExpression instead
            RuleFor(m => m.Notes).NotEmpty().NoDigits().When(m => !String.IsNullOrWhiteSpace(m.Notes));
            RuleFor(m => m.EntryID).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
            RuleForEach(m => m.Examples).SetValidator(new ExampleDtoValidator()).When(m => m.Examples.Count > 0);
        }
    }
}
