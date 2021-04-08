using Domain.Dto;
using Domain.Messages;
using Domain.Models;
using Domain.Repository;
using FluentValidation;
using Msg = Domain.ValidationErrorMessages;

namespace Domain.Validation
{
    public class CreateWordValidator : BaseValidator<CreateWordCommand>
    {
        public CreateWordValidator(IUnitOfWork uow):base(uow)
        {
            SetPropertyRules();
            SetRelationRules();
        }

        private void SetPropertyRules()
        {
            RuleFor(w => w.SourceLanguageName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(MessageConstants.NOT_EMPTY)
                .NoDigitsNoSpaces();
            
            RuleFor(w => w.Value).Cascade(CascadeMode.Stop).NotEmpty().NoDigits();
            RuleForEach(w => w.Properties).SetValidator(new WordPropertyDtoValidator());
        }

        private void SetRelationRules()
        {
            var langRepo = uow.Languages;
            
            //im not gonna check if there's a duplicate word because i think that should be the user problem, not domain's one
            RuleFor(m => m.SourceLanguageName)
                .Must(langRepo.ExistsByPrimaryKey)
                .WithMessage(name => Msg.EntityDoesNotExistByForeignKey<Word, Language>(l => l.Name, name));
        }
    }
}
