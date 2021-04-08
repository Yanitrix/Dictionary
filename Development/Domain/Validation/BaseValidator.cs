using Domain.Repository;
using FluentValidation;

namespace Domain.Validation
{
    public abstract class BaseValidator<T> : AbstractValidator<T>
    {
        protected readonly IUnitOfWork uow;

        protected BaseValidator(IUnitOfWork uow)
        {
            this.uow = uow;
        }
    }
}