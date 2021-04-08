using Domain.Dto;
using Domain.Models;
using Domain.Repository;
using FluentValidation;
using Msg = Domain.ValidationErrorMessages;

namespace Domain.Validation
{
    public class DeleteLanguageValidator : BaseValidator<DeleteLanguageCommand>
    {
        public DeleteLanguageValidator(IUnitOfWork uow) : base(uow)
        {
            var repo = uow.Languages;

            RuleFor(x => x.PrimaryKey)
                .Must(repo.ExistsByPrimaryKey)
                .WithMessage(Msg.EntityDoesNotExistByPrimaryKey<Language>());
        }
        
        
    }
}