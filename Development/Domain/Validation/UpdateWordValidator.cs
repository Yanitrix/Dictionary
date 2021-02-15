using Domain.Dto;
using FluentValidation;

namespace Domain.Validation
{
    public class UpdateWordValidator : AbstractValidator<UpdateWord>
    {
        public UpdateWordValidator()
        {
            RuleFor(w => w.Value).Cascade(CascadeMode.Stop).NotEmpty().NoDigits();
            RuleForEach(w => w.Properties).SetValidator(new WordPropertyDtoValidator());
        }
    }
}
