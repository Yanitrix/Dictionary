using Service.Repository;
using Data.Models;
using System;
using Msg = Commons.ValidationErrorMessages;

namespace Service
{
    public class WordService : IService<Word>
    {
        private readonly IWordRepository repo;
        private readonly ILanguageRepository langRepo;
        private IValidationDictionary validationDictionary;

        public WordService(IUnitOfWork uow)
        {
            this.repo = uow.Words;
            this.langRepo = uow.Languages;
        }

        public IValidationDictionary TryAdd(Word entity)
        {
            this.validationDictionary = IValidationDictionary.New();

            CheckLanguage(entity);
            CheckValueAndProperties(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);

            return validationDictionary;
        }

        public IValidationDictionary TryUpdate(Word entity)
        {
            this.validationDictionary = IValidationDictionary.New();

            //check if entity already exists
            if (!repo.ExistsByID(entity.ID))
            {
                validationDictionary.AddError(Msg.DOESNT_EXIST, Msg.DOESNT_EXIST_DESC<Word>());
                return validationDictionary;
            }

            //We don't check for Language because it cannot be updated.
            CheckValueAndProperties(entity);

            if (validationDictionary.IsValid)
                repo.Update(entity);

            return validationDictionary;
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
                validationDictionary.AddError(Msg.NOTFOUND<Language>(), Msg.NOTFOUND_DESC<Word, Language>(l => l.Name, entity.SourceLanguageName));
                return;
            }
        }
    }
}
