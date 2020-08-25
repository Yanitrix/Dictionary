using Commons;
using Data.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Validation
{
    public class LanguageValidator : AbstractValidator<Language>
    {
        public LanguageValidator()
        {
            RuleFor(l => l.Name).NotEmpty().Matches(RegexConstants.ONE_WORD_REGEX);
        }
    }
}
