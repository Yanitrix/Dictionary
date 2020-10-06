﻿using Commons;
using Data.Models;
using FluentValidation;

namespace Service.Validation
{
    public class WordValidator : AbstractValidator<Word>
    {
        public WordValidator()
        {
            RuleFor(w => w.ID).Empty().WithMessage(MessageConstants.EMPTY_ID);

            RuleFor(w => w.SourceLanguageName).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY).NoDigitsNoSpaces();

            RuleFor(w => w.Value).NotEmpty().NoDigits();
            RuleFor(w => w.Properties).NotEmpty().WithMessage("Collection of WordProperties cannot be empty");

            RuleForEach(w => w.Properties).SetValidator(new WordPropertyValidator());
        }
    }
}
