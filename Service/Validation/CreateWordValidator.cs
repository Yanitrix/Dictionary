using Commons;
using Data.Dto;
using FluentValidation;

namespace Service.Validation
{
    public class CreateWordValidator : AbstractValidator<CreateWord>
    {

        public CreateWordValidator()
        {
            RuleFor(w => w.SourceLanguageName).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY).NoDigitsNoSpaces();
            RuleFor(w => w.Value).NotEmpty().NoDigits();
            RuleForEach(w => w.Properties).SetValidator(new WordPropertyDtoValidator());
        }
    }
}
