using Service.Repository;
using Data.Models;
using FluentValidation;
using Msg = Commons.ValidationErrorMessages;


namespace Service
{
    public class LanguageService : ServiceBase<Language>
    {
        private readonly ILanguageRepository repo;

        public LanguageService(UnitOfWork uow, AbstractValidator<Language> _v) :base(_v)
        {
            this.repo = uow.Languages;
        }

        public override IValidationDictionary TryAdd(Language entity)
        {
            if (!IsValid(entity).IsValid) return validationDictionary;

            if (repo.ExistsByName(entity.Name))
            {
                validationDictionary.AddError(Msg.DUPLICATE, Msg.DUPLICATE_LANGUAGE_DESC);
            }

            if (validationDictionary.IsValid)
            {
                repo.Create(entity);
            }

            return validationDictionary;
        }

        public override IValidationDictionary TryUpdate(Language entity)
        {
            validationDictionary = new ValidationDictionary();

            validationDictionary.AddError(Msg.CANNOT_UPDATE, Msg.CANNOT_UPDATE_LANGUAGE_DESC);

            return validationDictionary;
        }
    }
}
