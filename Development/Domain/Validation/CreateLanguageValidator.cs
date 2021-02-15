using Domain.Dto;
using FluentValidation;

namespace Domain.Validation
{
    public class CreateLanguageValidator : AbstractValidator<CreateLanguage>
    {
        public CreateLanguageValidator()
        {
            RuleFor(l => l.Name).Cascade(CascadeMode.Stop).NotEmpty().NoDigitsNoSpaces();
        }
    }
}
