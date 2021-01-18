using Data.Dto;
using FluentValidation;

namespace Service.Validation
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
