using Data.Models;
using Service.Repository;
using System;
using Msg = Commons.ValidationErrorMessages;

namespace Service
{
    public class EntryService : IService<Entry>
    {
        private readonly IWordRepository wordRepo;
        private readonly IDictionaryRepository dictRepo;
        private readonly IEntryRepository repo;
        private IValidationDictionary validationDictionary;

        public EntryService(IUnitOfWork uow)
        {
            this.wordRepo = uow.Words;
            this.dictRepo = uow.Dictionaries;
            this.repo = uow.Entries;
        }

        public IValidationDictionary TryAdd(Entry entity)
        {
            this.validationDictionary = IValidationDictionary.New();

            CheckAll(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);

            return validationDictionary;
        }

        public IValidationDictionary TryUpdate(Entry entity)
        {
            this.validationDictionary = IValidationDictionary.New();

            //check if exists
            if (!repo.ExistsByID(entity.ID))
            {
                validationDictionary.AddError(Msg.DOESNT_EXIST, Msg.DOESNT_EXIST_DESC<Entry>());
                return validationDictionary;
            }

            //check if has meanings. if it does, cancel updating
            if (repo.HasMeanings(entity.ID))
            {
                validationDictionary.AddError(Msg.CANNOT_UPDATE, Msg.CANNOT_UPDATE_ENTRY_DESC);
                return validationDictionary;
            }

            CheckAll(entity);

            if (validationDictionary.IsValid)
                repo.Update(entity);

            return validationDictionary;
        }

        private void CheckAll(Entry entity)
        {
            //check if word exists
            if (!wordRepo.ExistsByID(entity.WordID))
            {
                validationDictionary.AddError(Msg.NOTFOUND<Word>(), Msg.NOTFOUND_DESC<Entry, Word>(w => w.ID, entity.WordID));
                return;
            }

            //one entry per word
            var existing = repo.GetOne(e => e.WordID == entity.WordID && e.DictionaryIndex == entity.DictionaryIndex);
            //If there's another entry another Entry with same WordID and DictionaryIndex, it's a possible duplicate
            //If the duplicate's ID is the same as the Entity's that means that the Entry is being updated. I think no error should be returned here.
            if (existing != null && existing.ID != entity.ID)
            {
                validationDictionary.AddError(Msg.DUPLICATE, Msg.DUPLICATE_ENTRY_DESC);
                return;
            }
            
            //check if dictionary exists
            if (!dictRepo.ExistsByIndex(entity.DictionaryIndex))
            {
                validationDictionary.AddError(Msg.NOTFOUND<Dictionary>(), Msg.NOTFOUND_DESC<Entry, Dictionary>(d => d.Index, entity.DictionaryIndex));
                return;
            }

            //check if word's source language is dictionary's language in
            var word = wordRepo.GetByID(entity.WordID);
            var dict = dictRepo.GetByIndex(entity.DictionaryIndex);
            if (!String.Equals(word.SourceLanguageName, dict.LanguageInName, StringComparison.Ordinal))
            {
                validationDictionary.AddError(Msg.LANGUAGES_NOT_MATCH, Msg.LANGUAGES_NOT_MATCH_DESC);
            }
        }
    }
}