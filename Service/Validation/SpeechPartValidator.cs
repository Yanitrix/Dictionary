using Data.Dto;
using Commons;
using Data.Models;
using FluentValidation;

namespace Api.Service.Validation
{
    public class SpeechPartValidator : AbstractValidator<SpeechPart>
    {
        public SpeechPartValidator()
        {
            RuleFor(sp => sp.Index).Empty().WithMessage(MessageConstants.EMPTY_INDEX);

            RuleFor(sp => sp.LanguageName).NotEmpty().NoDigitsNoSpaces();
            RuleFor(sp => sp.Name).NotEmpty().NoDigitsNoSpaces();
            RuleFor(sp => sp.Properties).NotEmpty();

            RuleForEach(sp => sp.Properties).SetValidator(new SpeechPartPropertyValidator());
        }

    }
}
