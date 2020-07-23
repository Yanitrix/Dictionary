﻿using Api.Dto;
using Commons;
using Dictionary_MVC.Models;
using FluentValidation;

namespace Api.Validation
{
    public class SpeechPartValidator : AbstractValidator<SpeechPart>
    {
        public SpeechPartValidator()
        {
            RuleFor(sp => sp.Index).Empty().WithMessage(MessageConstants.EMPTY_INDEX);

            RuleFor(sp => sp.LanguageName).NotEmpty().Matches(RegexConstants.ONE_WORD_REGEX);
            RuleFor(sp => sp.Name).NotEmpty().Matches(RegexConstants.ONE_WORD_REGEX);
            RuleFor(sp => sp.Properties).NotEmpty();

            RuleForEach(sp => sp.Properties).SetValidator(new SpeechPartPropertyValidator());
        }

    }
}
