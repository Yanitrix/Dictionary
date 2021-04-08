using Domain.Dto;
using Domain.Messages;
using FluentValidation;
using System;
using Domain.Repository;
using Msg = Domain.ValidationErrorMessages;

namespace Domain.Validation
{
    public class CreateDictionaryValidator : BaseValidator<CreateDictionaryCommand>
    {
        public CreateDictionaryValidator(IUnitOfWork uow) : base(uow)
        {
            SetPropertyRules();
            SetRelationRules();
        }

        private void SetPropertyRules()
        {
            RuleFor(d => d.LanguageIn)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NoDigits()
                .WithMessage(MessageConstants.NOT_EMPTY);
            
            RuleFor(d => d.LanguageOut)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NoDigits()
                .WithMessage(MessageConstants.NOT_EMPTY);
            
            RuleFor(d => d.LanguageIn)
                .NotEqual(d => d.LanguageOut, StringComparer.OrdinalIgnoreCase)
                .WithMessage("LanguageIn and LanguageOut cannot be same");
        }

        private void SetRelationRules()
        {
            IDictionaryRepository dictRepo = uow.Dictionaries;
            ILanguageRepository langRepo = uow.Languages;

            RuleFor(m => new {m.LanguageIn, m.LanguageOut})
                .Must(langs => langRepo.ExistsByPrimaryKey(langs.LanguageIn) && langRepo.ExistsByPrimaryKey(langs.LanguageOut))
                .WithMessage(langs => Msg.LanguagesNotFoundDesc(langs.LanguageIn, langs.LanguageOut));

            RuleFor(m => new {m.LanguageIn, m.LanguageOut})
                .Must(m => !dictRepo.ExistsByLanguages(m.LanguageIn, m.LanguageOut))
                .WithMessage(Msg.DUPLICATE_DICTIONARY_DESC);
        }
    }
}
