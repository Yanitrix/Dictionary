using Domain.Dto;
using Domain.Messages;
using Domain.Models;
using Domain.Repository;
using FluentValidation;
using Msg = Domain.ValidationErrorMessages;

namespace Domain.Validation
{
    public class UpdateFreeExpressionValidator : BaseValidator<UpdateFreeExpressionCommand>
    {
        public UpdateFreeExpressionValidator(IUnitOfWork uow):base(uow)
        {
            SetPropertyRules();
            SetRelationRules();
        }

        private void SetPropertyRules()
        {
            RuleFor(ex => ex.Text).Cascade(CascadeMode.Stop).NotEmpty().NoDigits();
            RuleFor(ex => ex.Translation).Cascade(CascadeMode.Stop).NotEmpty().NoDigits();

            RuleFor(ex => ex.DictionaryIndex).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
        }

        private void SetRelationRules()
        {
            var dictRepo = uow.Dictionaries;
            var repo = uow.FreeExpressions;

            RuleFor(m => m.ID)
                .Must(repo.ExistsByPrimaryKey)
                .WithMessage(Msg.ThereIsNothingToUpdate<FreeExpression>());
            
            RuleFor(m => m.DictionaryIndex)
                .Must(dictRepo.ExistsByPrimaryKey)
                .WithMessage(idx => Msg.EntityDoesNotExistByForeignKey<FreeExpression, Dictionary>(m => m.Index, idx));
        }
    }
}
