using Domain.Dto;
using Domain.Models;
using Domain.Repository;
using Service.Mapper;
using Service.Service.Abstract;
using System;
using System.Collections.Generic;
using Msg = Service.ValidationErrorMessages;

namespace Service
{
    public class LanguageService : ServiceBase, ILanguageService
    {
        private readonly ILanguageRepository repo;

        public LanguageService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
        {
            this.repo = uow.Languages;
        }

        public GetLanguage Get(String name) => Map(repo.GetByNameWithWords(name));

        public IEnumerable<LanguageWordCount> AllWithWordCount() => repo.AllWithWordCount();

        public ValidationResult Add(CreateLanguage dto)
        {
            var entity = mapper.Map<CreateLanguage, Language>(dto);

            var validationDictionary = ValidationResult.New(entity);

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

        public ValidationResult Delete(String name)
        {
            var result = ValidationResult.New(name);
            var indb = repo.GetByName(name);

            if(indb == null)
            {
                result.AddError(Msg.EntityNotFound<Language>(), Msg.EntityDoesNotExistByPrimaryKey<Language>());
            }
            else
            {
                repo.Delete(indb);
            }

            return result;
        }

        private GetLanguage Map(Language obj) => mapper.Map<Language, GetLanguage>(obj);
    }
}
