using Domain.Dto;
using Domain.Models;
using Domain.Repository;
using FluentValidation;
using Msg = Domain.ValidationErrorMessages;

namespace Domain.Validation
{
    public class DeleteFreeExpressionValidator : BaseValidator<DeleteFreeExpressionCommand>
    {
        public DeleteFreeExpressionValidator(IUnitOfWork uow) : base(uow)
        {
            var repo = uow.FreeExpressions;

            RuleFor(x => x.PrimaryKey)
                .Must(repo.ExistsByPrimaryKey)
                .WithMessage(Msg.EntityDoesNotExistByPrimaryKey<FreeExpression>());

        }
    }
}