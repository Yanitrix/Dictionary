using Service.Repository;
using Data.Models;
using Msg = Commons.ValidationErrorMessages;
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

        public IValidationDictionary Add(Language entity)
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

        public IValidationDictionary Update(Language entity)
        {
            var validationDictionary = IValidationDictionary.New();

            validationDictionary.AddError(Msg.CANNOT_UPDATE, Msg.CANNOT_UPDATE_LANGUAGE_DESC);

            return validationDictionary;
        }

        public IValidationDictionary Delete(String name)
        {
            var result = IValidationDictionary.New();
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
