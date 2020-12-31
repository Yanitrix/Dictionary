using Service.Repository;
using Data.Models;
using Msg = Commons.ValidationErrorMessages;


namespace Service
{
    public class LanguageService : IService<Language>
    {
        private readonly ILanguageRepository repo;

        public LanguageService(IUnitOfWork uow)
        {
            this.repo = uow.Languages;
        }

        public IValidationDictionary TryAdd(Language entity)
        {
            var validationDictionary = IValidationDictionary.New();

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

        public IValidationDictionary TryUpdate(Language entity)
        {
            var validationDictionary = IValidationDictionary.New();

            validationDictionary.AddError(Msg.CANNOT_UPDATE, Msg.CANNOT_UPDATE_LANGUAGE_DESC);

            return validationDictionary;
        }
    }
}
