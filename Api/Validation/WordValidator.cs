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
            //TODO
            //RuleFor(w => w.SourceLanguageName).Matches("^[a-zA-Z]+$");    won't check it because service will check if the language actually exists in database
            //RuleFor(w => w.SpeechPartName)                                //wont check it, same reason as above
            //RuleForEach(w => w.Properties)                                ------------ '' ------------

            RuleFor(w => w.Value).NotEmpty().Matches(RegexConstants.ALPHA_REGEX);

        }
    }
}
