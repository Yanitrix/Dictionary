using Commons;
using Dictionary_MVC.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Validation
{
    public class ExpressionValidator : AbstractValidator<Expression>
    {
        public ExpressionValidator()
        {
            RuleFor(ex => ex.ID).Empty().WithMessage(MessageConstants.EMPTY_ID);

            RuleFor(ex => ex.Text).NotEmpty().Matches(RegexConstants.ALPHA_REGEX);
            RuleFor(ex => ex.Translation).NotEmpty().Matches(RegexConstants.ALPHA_REGEX);
        }
    }
}
