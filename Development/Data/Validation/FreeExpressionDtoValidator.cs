using Data.Dto;
using Data.Messages;
using FluentValidation;

namespace Data.Validation
{
    public class FreeExpressionDtoValidator : AbstractValidator<CreateOrUpdateFreeExpression>
    {
        public FreeExpressionDtoValidator()
        {
            RuleFor(ex => ex.Text).Cascade(CascadeMode.Stop).NotEmpty().NoDigits();
            RuleFor(ex => ex.Translation).Cascade(CascadeMode.Stop).NotEmpty().NoDigits();

            RuleFor(ex => ex.DictionaryIndex).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
        }
    }
}
