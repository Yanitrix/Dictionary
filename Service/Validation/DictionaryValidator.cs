using Commons;
using Data.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service.Validation
{
    public class DictionaryValidator : AbstractValidator<Dictionary>
    {
        public DictionaryValidator()
        {
            RuleFor(d => d.Index).Empty().WithMessage(MessageConstants.EMPTY_INDEX);
            RuleFor(d => d.FreeExpressions).Empty().WithMessage(MessageConstants.EMPTY);
            RuleFor(d => d.Entries).Empty().WithMessage(MessageConstants.EMPTY);

            RuleFor(d => d.LanguageInName).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
            RuleFor(d => d.LanguageOutName).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
            RuleFor(d => d.LanguageInName).NotEqual(d => d.LanguageOutName, StringComparer.OrdinalIgnoreCase).WithMessage("LanguageIn and LanguageOut cannot be same");
        }
    }
}
