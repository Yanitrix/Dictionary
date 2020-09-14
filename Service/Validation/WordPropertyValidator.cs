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

            RuleFor(wp => wp.Name).NotEmpty().NoDigitsNoSpaces();
            RuleFor(wp => wp.Values).NotEmpty();
            RuleForEach(wp => wp.Values).NotEmpty().NoDigits();
        }
    }
}
