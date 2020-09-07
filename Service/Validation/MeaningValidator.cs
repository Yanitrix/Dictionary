using Commons;
using Data.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Service.Validation
{
    public class MeaningValidator : AbstractValidator<Meaning>
    {
        public MeaningValidator()
        {
            RuleFor(m => m.ID).Empty().WithMessage(MessageConstants.EMPTY_ID);

            RuleFor(m => m.EntryID).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);

            RuleFor(m => m.Value).NotEmpty().NoDigits(); //cannot be empty, if user wants to use only examples then they're encouraged to use dictionary-level Expression instead
            RuleFor(m => m.Notes).NotEmpty().NoDigits().When(m => !String.IsNullOrWhiteSpace(m.Notes));

            RuleForEach(m => m.Examples).SetValidator(new ExpressionValidator()).When(m => m.Examples.Any());
        }
    }
}
