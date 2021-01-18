using Commons;
using Data.Dto;
using FluentValidation;

namespace Service.Validation
{
    public class UpdateWordValidator : AbstractValidator<UpdateWord>
    {
        public UpdateWordValidator()
        {
            RuleFor(w => w.Value).NotEmpty().NoDigits();
            RuleForEach(w => w.Properties).SetValidator(new WordPropertyDtoValidator());
        }
    }
}
