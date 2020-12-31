using Commons;
using Data.Dto;
using FluentValidation;

namespace Service.Validation
{
    class UpdateWordValidator : AbstractValidator<UpdateWord>
    {
        public UpdateWordValidator()
        {
            RuleFor(w => w.SourceLanguageName).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY).NoDigitsNoSpaces();
            RuleForEach(w => w.Properties).SetValidator(new WordPropertyDtoValidator());
        }
    }
}
