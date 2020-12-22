using Commons;
using Data.Models;
using FluentValidation;

namespace Service.Validation
{
    public class ExampleValidator : AbstractValidator<Example>
    {
        public ExampleValidator()
        {
            RuleFor(ex => ex.ID).Empty().WithMessage(MessageConstants.EMPTY_ID);

            RuleFor(ex => ex.Text).NotEmpty().NoDigits();
            RuleFor(ex => ex.Translation).NotEmpty().NoDigits();

        }
    }
}
