using Domain.Dto;
using Domain.Messages;
using FluentValidation;

namespace Domain.Validation
{
    public class CreateFreeExpressionValidator : AbstractValidator<CreateFreeExpression>
    {
        public CreateFreeExpressionValidator()
        {
            RuleFor(ex => ex.Text).Cascade(CascadeMode.Stop).NotEmpty().NoDigits();
            RuleFor(ex => ex.Translation).Cascade(CascadeMode.Stop).NotEmpty().NoDigits();

            RuleFor(ex => ex.DictionaryIndex).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
        }
    }
}
