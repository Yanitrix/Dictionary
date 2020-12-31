using Commons;
using Data.Dto;
using FluentValidation;
using System;

namespace Service.Validation
{
    public class CreateDictionaryValidator : AbstractValidator<CreateDictionary>
    {
        public CreateDictionaryValidator()
        {
            RuleFor(d => d.LanguageInName).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
            RuleFor(d => d.LanguageOutName).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
            RuleFor(d => d.LanguageInName).NotEqual(d => d.LanguageOutName, StringComparer.OrdinalIgnoreCase).WithMessage("LanguageIn and LanguageOut cannot be same");
        }
    }
}
