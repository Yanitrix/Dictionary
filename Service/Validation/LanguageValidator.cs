using Data.Models;
using FluentValidation;

namespace Service.Validation
{
    public class LanguageValidator : AbstractValidator<Language>
    {
        public LanguageValidator()
        {
            RuleFor(l => l.Name).NotEmpty().NoDigitsNoSpaces();
        }
    }
}
