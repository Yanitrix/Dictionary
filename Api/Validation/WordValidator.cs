using Commons;
using Dictionary_MVC.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Validation
{
    public class WordValidator : AbstractValidator<Word>
    {
        public WordValidator()
        {
            RuleFor(w => w.ID).Empty().WithMessage(MessageConstants.EMPTY_ID);

            RuleFor(w => w.Value).NotEmpty().Matches(RegexConstants.ALPHA_REGEX);

        }
    }
}
