using Data.Models;
using Service.Repository;
using Msg = Commons.ValidationErrorMessages;


namespace Service
{
    public class DictionaryService : IService<Dictionary>
    {
        private readonly ILanguageRepository langRepo;
        private readonly IDictionaryRepository repo;

        public DictionaryService(IUnitOfWork uow)
        {
            this.langRepo = uow.Languages;
            this.repo = uow.Dictionaries;
        }

        public IValidationDictionary TryAdd(Dictionary entity)
        {
            var validationDictionary = IValidationDictionary.New();

            //check if dict already exists
            if (repo.ExistsByLanguages(entity.LanguageInName, entity.LanguageOutName))
            {
                validationDictionary.AddError(Msg.DUPLICATE, Msg.DUPLICATE_DICTIONARY_DESC);
                return validationDictionary;
            }
            //check if langueges exist
            if (!langRepo.ExistsByName(entity.LanguageInName) || !langRepo.ExistsByName(entity.LanguageOutName))
            {
                validationDictionary.AddError(Msg.NOTFOUND<Language>(), Msg.LANGS_NOTFOUND_DESC(entity.LanguageInName, entity.LanguageOutName));
                return validationDictionary;
            }

            if (validationDictionary.IsValid)
                repo.Create(entity);

            return validationDictionary;
        }

        public IValidationDictionary TryUpdate(Dictionary entity)
        {
            var validationDictionary = IValidationDictionary.New();

            validationDictionary.AddError(Msg.CANNOT_UPDATE, Msg.CANNOT_UPDATE_DICTIONARY_DESC);

            return validationDictionary;
        }
    }
}
