using Commons;
using Dictionary_MVC.Models;
using FluentValidation;

namespace Api.Validation
{
    public class SpeechPartPropertyValidator : AbstractValidator<SpeechPartProperty>
    {
        public SpeechPartPropertyValidator()
        {
            RuleFor(part => part.ID).Empty().WithMessage(MessageConstants.EMPTY_ID);
            RuleFor(part => part.SpeechPartIndex).Empty().WithMessage(MessageConstants.EMPTY_INDEX);
            
            RuleFor(part => part.Name).NotEmpty().Matches(RegexConstants.ONE_WORD_REGEX);
            RuleFor(part => part.PossibleValues).NotEmpty();
            RuleForEach(part => part.PossibleValues).NotEmpty().Matches(RegexConstants.ONE_WORD_REGEX);
        }
    }
}
