using Domain.Dto;
using Domain.Models;
using Domain.Repository;
using Service.Mapper;
using Service.Service.Abstract;
using System.Collections.Generic;
using System.Linq;
using Msg = Service.ValidationErrorMessages;

namespace Service
{
    public class DictionaryService : ServiceBase, IDictionaryService
    {
        private readonly ILanguageRepository langRepo;
        private readonly IDictionaryRepository repo;

        public DictionaryService(IUnitOfWork uow, IMapper mapper):base(uow, mapper)
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

        public ValidationResult Add(CreateDictionary dto)
        {
            var entity = mapper.Map<CreateDictionary, Dictionary>(dto);

            var validationDictionary = ValidationResult.New(entity);

            //check if dict already exists
            if (repo.ExistsByLanguages(entity.LanguageInName, entity.LanguageOutName))
            {
                validationDictionary.AddError(Msg.DUPLICATE, Msg.DUPLICATE_DICTIONARY_DESC);
                return validationDictionary;
            }
            //check if langueges exist
            if (!langRepo.ExistsByName(entity.LanguageInName) || !langRepo.ExistsByName(entity.LanguageOutName))
            {
                validationDictionary.AddError(Msg.EntityNotFound<Language>(), Msg.LanguagesNotFoundDesc(entity.LanguageInName, entity.LanguageOutName));
                return validationDictionary;
            }

            if (validationDictionary.IsValid)
                repo.Create(entity);

            return validationDictionary;
        }

        public ValidationResult Delete(int index)
        {
            var result = ValidationResult.New(index);

            var inDB = repo.GetByIndex(index);

            if (inDB == null)
            {
                result.AddError(Msg.EntityNotFound<Dictionary>(), Msg.EntityDoesNotExistByPrimaryKey<Dictionary>());
            }
            else
            {
                repo.Delete(inDB);
            }

            return result;
        }
    }
}
