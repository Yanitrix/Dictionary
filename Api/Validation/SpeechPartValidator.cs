using Api.Dto;
using Dictionary_MVC.Models;
using FluentValidation;

namespace Api.Validation
{
    public class SpeechPartValidator : AbstractValidator<SpeechPart>
    {
        public SpeechPartValidator()
        {
            RuleFor(sp => sp.LanguageName).NotEmpty().Matches("^[a-zA-Z]+$");
            RuleFor(sp => sp.Name).NotEmpty().Matches("^[a-zA-Z]+$");
            RuleFor(sp => sp.Properties).NotEmpty();
            RuleForEach(sp => sp.Properties).SetValidator(new SpeechPartPropertyValidator());
        }

    }
}
