using Commons;
using Data.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service.Validation
{
    public class ExpressionValidator : AbstractValidator<Expression>
    {
        public ExpressionValidator()
        {
            RuleFor(ex => ex.ID).Empty().WithMessage(MessageConstants.EMPTY_ID);

            RuleFor(ex => ex.Text).NotEmpty().NoDigits();
            RuleFor(ex => ex.Translation).NotEmpty().NoDigits();

            RuleFor(ex => ex.DictionaryIndex).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY).When(ex => ex.MeaningID != null);
            RuleFor(ex => ex.MeaningID).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY).When(ex => ex.DictionaryIndex != null);

        }
    }
}
