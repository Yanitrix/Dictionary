using Domain.Dto;
using FluentValidation;

namespace Domain.Validation
{
    public class WordPropertyDtoValidator : AbstractValidator<WordPropertyDto>
    {
        public WordPropertyDtoValidator()
        {
            RuleFor(wp => wp.Name).Cascade(CascadeMode.Stop).NotEmpty().NoDigits();
            RuleForEach(wp => wp.Values).Cascade(CascadeMode.Stop).NotEmpty().NoDigits();
        }
    }
}
