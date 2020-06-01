using Dictionary_MVC.Models;
using FluentValidation;

namespace Api.Validation
{
    public class SpeechPartPropertyValidator : AbstractValidator<SpeechPartProperty>
    {
        public SpeechPartPropertyValidator()
        {
            RuleFor(part => part.ID).Empty().WithMessage("ID must be empty");
            RuleFor(part => part.SpeechPartIndex).Empty().WithMessage("Speech part index must be empty");
            
            RuleFor(part => part.Name).NotEmpty().Matches("^[a-zA-Z]+$");
            RuleFor(part => part.PossibleValues).NotEmpty();
            RuleForEach(part => part.PossibleValues).NotEmpty().Matches("^[a-zA-Z]+$");
        }
    }
}
