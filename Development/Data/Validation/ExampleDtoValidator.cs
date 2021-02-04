using Data.Dto;
using FluentValidation;

namespace Data.Validation
{
    public class ExampleDtoValidator : AbstractValidator<ExampleDto>
    {
        public ExampleDtoValidator()
        {
            RuleFor(ex => ex.Text).Cascade(CascadeMode.Stop).NotEmpty().NoDigits();
            RuleFor(ex => ex.Translation).Cascade(CascadeMode.Stop).NotEmpty().NoDigits();
        }
    }
}
