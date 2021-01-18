using Data.Dto;
using FluentValidation;
using System;

namespace Service.Validation
{
    public class UpdateMeaningValidator : AbstractValidator<UpdateMeaning>
    {
        public UpdateMeaningValidator()
        {
            RuleFor(m => m.Value).Cascade(CascadeMode.Stop).NotEmpty().NoDigits(); //cannot be empty, if user wants to use only examples then they're encouraged to use FreeExpression instead
            RuleFor(m => m.Notes).Cascade(CascadeMode.Stop).NotEmpty().NoDigits().When(m => !String.IsNullOrWhiteSpace(m.Notes));
            RuleForEach(m => m.Examples).SetValidator(new ExampleDtoValidator()).When(m => m.Examples.Count > 0);
        }
    }
}
