using Data.Models;
using FluentValidation;
using Service.Repository;
using Msg = Commons.ValidationErrorMessages;


namespace Service
{
    public class DictionaryService : ServiceBase<Dictionary>
    {
        private readonly ILanguageRepository langRepo;
        private readonly IDictionaryRepository repo;

        public DictionaryService(IUnitOfWork uow, AbstractValidator<Dictionary> validator)
            :base(validator)
        {
            this.langRepo = uow.Languages;
            this.repo = uow.Dictionaries;
        }

        public override IValidationDictionary TryAdd(Dictionary entity)
        {
            if (!IsValid(entity).IsValid) return validationDictionary;

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

        public override IValidationDictionary TryUpdate(Dictionary entity)
        {
            validationDictionary = new ValidationDictionary();

            validationDictionary.AddError(Msg.CANNOT_UPDATE, Msg.CANNOT_UPDATE_DICTIONARY_DESC);

            return validationDictionary;
        }
    }
}
