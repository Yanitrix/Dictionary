using Commons;
using Data.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service.Validation
{
    public class WordPropertyValidator : AbstractValidator<WordProperty>
    {
        public WordPropertyValidator()
        {
            RuleFor(wp => wp.ID).Empty().WithMessage(MessageConstants.EMPTY_ID);

            RuleFor(wp => wp.WordID).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
            RuleFor(wp => wp.Name).NotEmpty().Matches(RegexConstants.ONE_WORD_REGEX);
            RuleFor(wp => wp.Value).NotEmpty().Matches(RegexConstants.ONE_WORD_REGEX);
        }
    }
}
