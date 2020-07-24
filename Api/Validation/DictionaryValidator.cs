using Commons;
using Dictionary_MVC.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Validation
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
        }
    }
}
