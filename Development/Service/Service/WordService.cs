using Domain.Repository;
using Domain.Models;
using System;
using Msg = Service.ValidationErrorMessages;
using System.Collections.Generic;

namespace Service
{
    public class WordService : IWordService
    {
        private readonly IWordRepository repo;
        private readonly ILanguageRepository langRepo;
        private ValidationResult validationDictionary;

        public WordService(IUnitOfWork uow)
        {
            this.repo = uow.Words;
            this.langRepo = uow.Languages;
        }

        public Word Get(int id) => repo.GetByID(id);

        public IEnumerable<Word> Get(String value) => repo.GetByValue(value, false);

        public ValidationResult Add(Word entity)
        {
            this.validationDictionary = ValidationResult.New(entity);

            CheckLanguage(entity);
            CheckValueAndProperties(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);

            return validationDictionary;
        }

        public ValidationResult Update(Word entity)
        {
            this.validationDictionary = ValidationResult.New(entity);

            //check if entity already exists
            if (!repo.ExistsByID(entity.ID))
            {
                validationDictionary.AddError(Msg.EntityNotFound(), Msg.ThereIsNothingToUpdate<Word>());
                return validationDictionary;
            }

            //We don't check for Language because it cannot be updated.
            CheckValueAndProperties(entity);

            if (validationDictionary.IsValid)
                repo.Update(entity);

            return validationDictionary;
        }

        public ValidationResult Delete(int id)
        {
            var result = ValidationResult.New(id);
            var indb = repo.GetByID(id);

            if(indb == null)
            {
                result.AddError(Msg.EntityNotFound<Word>(), Msg.EntityDoesNotExistByPrimaryKey<Word>());
            }
            else
            {
                repo.Delete(indb);
            }

            return result;
        }

        private void CheckValueAndProperties(Word entity)
        {
            //check if there's a word with same set of WordProperties
            var similar = repo.GetByLanguageAndValue(entity.SourceLanguageName, entity.Value, false);
            foreach (var i in similar)
            {
                if (entity.Properties.SetEquals(i.Properties) && String.Equals(entity.Value, i.Value, StringComparison.OrdinalIgnoreCase))
                {
                    validationDictionary.AddError(Msg.DUPLICATE, Msg.DUPLICATE_WORD_DESC);
                }
            }
        }

        private void CheckLanguage(Word entity)
        {
            //check if language exists
            if (!langRepo.ExistsByName(entity.SourceLanguageName))
            {
                validationDictionary.AddError(Msg.EntityNotFound<Language>(), Msg.EntityDoesNotExistByForeignKey<Word, Language>(l => l.Name, entity.SourceLanguageName));
                return;
            }
        }
    }
}
