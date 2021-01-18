using Commons;
using Data.Dto;
using FluentValidation;

namespace Service.Validation
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
