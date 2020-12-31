using Commons;
using Data.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validation
{
    public class FreeExpressionDtoValidator : AbstractValidator<CreateOrUpdateFreeExpression>
    {
        public FreeExpressionDtoValidator()
        {
            RuleFor(ex => ex.Text).NotEmpty().NoDigits();
            RuleFor(ex => ex.Translation).NotEmpty().NoDigits();

            RuleFor(ex => ex.DictionaryIndex).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
        }
    }
}
