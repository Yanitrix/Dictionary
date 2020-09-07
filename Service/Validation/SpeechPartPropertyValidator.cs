using Commons;
using Data.Models;
using FluentValidation;

namespace Api.Service.Validation
{
    public class SpeechPartPropertyValidator : AbstractValidator<SpeechPartProperty>
    {
        public SpeechPartPropertyValidator()
        {
            RuleFor(part => part.ID).Empty().WithMessage(MessageConstants.EMPTY_ID);

            RuleFor(part => part.Name).NotEmpty().NoDigitsNoSpaces();
            RuleFor(part => part.PossibleValues).NotEmpty();
            RuleForEach(part => part.PossibleValues).NotEmpty().NoDigitsNoSpaces();
        }
    }
}
