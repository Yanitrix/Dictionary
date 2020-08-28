using Commons;
using Data.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service.Validation
{
    public class WordValidator : AbstractValidator<Word>
    {
        public WordValidator()
        {
            RuleFor(w => w.ID).Empty().WithMessage(MessageConstants.EMPTY_ID);

            RuleFor(w => w.SourceLanguageName).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY).Matches(RegexConstants.ONE_WORD_REGEX);
            RuleFor(w => w.SpeechPartName).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY).Matches(RegexConstants.ONE_WORD_REGEX);

            RuleFor(w => w.Value).NotEmpty().Matches(RegexConstants.ALPHA_REGEX);

            RuleFor(w => w.Properties).NotEmpty().WithMessage("Collection of WordProperties cannot be empty");

            RuleForEach(w => w.Properties).SetValidator(new WordPropertyValidator());

        }
    }
}
