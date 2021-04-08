using Domain.Dto;
using Domain.Repository;
using FluentValidation;
using Msg = Domain.ValidationErrorMessages;

namespace Domain.Validation
{
    public class CreateLanguageValidator : BaseValidator<CreateLanguageCommand>
    {
        public CreateLanguageValidator(IUnitOfWork uow):base(uow)
        {
            var repo = uow.Languages;
            
            RuleFor(l => l.Name).Cascade(CascadeMode.Stop).NotEmpty().NoDigitsNoSpaces();
            
            RuleFor(l => l.Name)
                .Must(name => !repo.ExistsByPrimaryKey(name))
                .WithMessage(Msg.DUPLICATE_LANGUAGE_DESC);
        }
    }
}
