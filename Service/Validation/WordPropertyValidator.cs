﻿using Commons;
using Data.Models;
using FluentValidation;

namespace Service.Validation
{
    public class WordPropertyValidator : AbstractValidator<WordProperty>
    {
        public WordPropertyValidator()
        {
            RuleFor(wp => wp.ID).Empty().WithMessage(MessageConstants.EMPTY_ID);

            RuleFor(wp => wp.Name).NotEmpty().NoDigits();
            RuleFor(wp => wp.Values).NotEmpty();
            RuleForEach(wp => wp.Values).NotEmpty().NoDigits();
        }
    }
}
