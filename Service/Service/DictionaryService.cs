using Data.Models;
using Service.Repository;
using System.Collections.Generic;
using System.Linq;
using Msg = Commons.ValidationErrorMessages;


namespace Service
{
    public class DictionaryService : IDictionaryService
    {
        private readonly ILanguageRepository langRepo;
        private readonly IDictionaryRepository repo;

        public DictionaryService(IUnitOfWork uow)
        {
            this.langRepo = uow.Languages;
            this.repo = uow.Dictionaries;
        }

        public Dictionary Get(int id) => repo.GetByIndex(id);
        //TODO test it
        public IEnumerable<Dictionary> GetContainingLanguage(string langIn, string langOut, string lang)
        {
            if ((langIn != null || langOut != null) && lang != null) return Enumerable.Empty<Dictionary>();
            if (langIn == null && langOut == null && lang == null) return repo.All();
            if (lang != null) return repo.GetAllByLanguage(lang);
            if (langIn != null && langOut == null) return repo.GetAllByLanguageIn(langIn);
            if (langIn == null && langOut != null) return repo.GetAllByLanguageOut(langOut);
            return new Dictionary[] { repo.GetByLanguageInAndOut(langIn, langOut) };
        }

        public IValidationDictionary Add(Dictionary entity)
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

        public IValidationDictionary Update(Dictionary entity)
        {
            var validationDictionary = IValidationDictionary.New();

            validationDictionary.AddError(Msg.CANNOT_UPDATE, Msg.CANNOT_UPDATE_DICTIONARY_DESC);

            return validationDictionary;
        }

        public IValidationDictionary Delete(int index)
        {
            var result = IValidationDictionary.New();

            var inDB = repo.GetByIndex(index);

            if (inDB == null)
            {
                result.AddError(Msg.NOTFOUND<Dictionary>(), Msg.DOESNT_EXIST_PK<Dictionary>());
            }
            else
            {
                repo.Delete(inDB);
            }

            return result;
        }
    }
}
