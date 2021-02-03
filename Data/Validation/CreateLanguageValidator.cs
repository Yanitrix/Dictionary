using Data.Dto;
using FluentValidation;

namespace Data.Validation
{
    public class CreateLanguageValidator : AbstractValidator<CreateLanguage>
    {
        public CreateLanguageValidator()
        {
            RuleFor(l => l.Name).Cascade(CascadeMode.Stop).NotEmpty().NoDigitsNoSpaces();
        }
    }
}
