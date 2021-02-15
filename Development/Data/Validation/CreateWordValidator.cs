using Data.Dto;
using Data.Messages;
using FluentValidation;

namespace Data.Validation
{
    public class CreateWordValidator : AbstractValidator<CreateWord>
    {
        public CreateWordValidator()
        {
            RuleFor(w => w.SourceLanguageName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY).NoDigitsNoSpaces();
            RuleFor(w => w.Value).Cascade(CascadeMode.Stop).NotEmpty().NoDigits();
            RuleForEach(w => w.Properties).SetValidator(new WordPropertyDtoValidator());
        }
    }
}
