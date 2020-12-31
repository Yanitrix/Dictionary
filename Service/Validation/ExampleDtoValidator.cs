using Data.Dto;
using FluentValidation;

namespace Service.Validation
{
    public class ExampleDtoValidator : AbstractValidator<ExampleDto>
    {
        public ExampleDtoValidator()
        {
            RuleFor(ex => ex.Text).NotEmpty().NoDigits();
            RuleFor(ex => ex.Translation).NotEmpty().NoDigits();
        }
    }
}
