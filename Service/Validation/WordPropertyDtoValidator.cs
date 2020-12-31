using Data.Dto;
using FluentValidation;

namespace Service.Validation
{
    class WordPropertyDtoValidator : AbstractValidator<WordPropertyDto>
    {
        public WordPropertyDtoValidator()
        {
            RuleFor(wp => wp.Name).NotEmpty().NoDigits();
            RuleFor(wp => wp.Values).NotEmpty();
            RuleForEach(wp => wp.Values).NotEmpty().NoDigits();
        }
    }
}
