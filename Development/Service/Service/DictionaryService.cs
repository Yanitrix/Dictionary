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

        public GetDictionary Get(int id) => mapper.Map<Dictionary, GetDictionary>(repo.GetByIndex(id));
        //TODO test it
        public IEnumerable<GetDictionary> GetContainingLanguage(string langIn, string langOut, string lang)
        {
            if ((langIn != null || langOut != null) && lang != null) return Enumerable.Empty<GetDictionary>();
            if (langIn == null && langOut == null && lang == null) return repo.All().Select(Map);
            if (lang != null) return repo.GetAllByLanguage(lang).Select(Map);
            if (langIn != null && langOut == null) return repo.GetAllByLanguageIn(langIn).Select(Map);
            if (langIn == null && langOut != null) return repo.GetAllByLanguageOut(langOut).Select(Map);
            return new GetDictionary[] { Map(repo.GetByLanguageInAndOut(langIn, langOut)) };
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

        private GetDictionary Map(Dictionary entity) => mapper.Map<Dictionary, GetDictionary>(entity);
    }
}
