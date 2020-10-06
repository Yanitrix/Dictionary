using FluentValidation;

namespace Service
{
    public abstract class ServiceBase<T> : IService<T>
    {
        protected IValidationDictionary validationDictionary;
        protected readonly AbstractValidator<T> validator;

        protected ServiceBase(AbstractValidator<T> validator)
        {
            this.validator = validator;
        }

        public virtual IValidationDictionary IsValid(T entity)
        {
            validationDictionary = new ValidationDictionary();

            var result = validator.Validate(entity);
            if (!result.IsValid)
            {
                foreach(var entry in result.Errors)
                {
                    validationDictionary.AddError(entry.PropertyName, entry.ErrorMessage);
                }
            }

            return validationDictionary;
        }

        public abstract IValidationDictionary TryAdd(T entity);

        public abstract IValidationDictionary TryUpdate(T entity);
    }
}
