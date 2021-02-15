using Data.Repository;
using Data.Models;
using Msg = Service.ValidationErrorMessages;
using Data.Dto;
using System;
using System.Collections.Generic;

namespace Service
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository repo;

        public LanguageService(IUnitOfWork uow)
        {
            this.repo = uow.Languages;
        }

        public Language Get(String name) => repo.GetByNameWithWords(name);

        public IEnumerable<LanguageWordCount> AllWithWordCount() => repo.AllWithWordCount();

        public ValidationResult Add(Language entity)
        {
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

        public ValidationResult Update(Language entity)
        {
            var validationDictionary = ValidationResult.New(entity);

            validationDictionary.AddError(Msg.CANNOT_UPDATE, Msg.CANNOT_UPDATE_LANGUAGE_DESC);

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
    }
}
