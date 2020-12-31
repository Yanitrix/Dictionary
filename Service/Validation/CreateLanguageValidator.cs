using Data.Dto;
using FluentValidation;

namespace Service.Validation.Commons
{
    public class CreateLanguageValidator : AbstractValidator<CreateLanguage>
    {
        public CreateLanguageValidator()
        {
            RuleFor(l => l.Name).NotEmpty().NoDigitsNoSpaces();
        }
    }
}
