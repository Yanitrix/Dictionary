using Commons;
using Data.Models;
using FluentValidation;

namespace Service.Validation
{
    public class FreeExpressionValidator : AbstractValidator<FreeExpression>
    {

        public FreeExpressionValidator()
        {
            RuleFor(ex => ex.ID).Empty().WithMessage(MessageConstants.EMPTY_ID);

            RuleFor(ex => ex.Text).NotEmpty().NoDigits();
            RuleFor(ex => ex.Translation).NotEmpty().NoDigits();

            RuleFor(ex => ex.DictionaryIndex).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
        }
    }
}
