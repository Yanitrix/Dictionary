using Api.Service;
using Data.Models;
using FluentValidation;
using Service.Service.Abstract;
using System;

namespace Service.Service
{
    public class EntryService : ServiceBase<Entry>
    {
        private readonly IWordRepository wordRepo;
        private readonly IDictionaryRepository dictRepo;
        private readonly IEntryRepository repo;

        public EntryService(IWordRepository wordRepo, IDictionaryRepository dictRepo, IEntryRepository repo, AbstractValidator<Entry> _v)
            : base(_v)
        {
            this.wordRepo = wordRepo;
            this.dictRepo = dictRepo;
            this.repo = repo;
        }

        public override IValidationDictionary TryAdd(Entry entity)
        {
            if (!IsValid(entity).IsValid) return validationDictionary;

            CheckAll(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);

            return validationDictionary;
        }

        public override IValidationDictionary TryUpdate(Entry entity)
        {
            if (!IsValid(entity).IsValid) return validationDictionary;

            //check if exists
            if (!repo.ExistsByID(entity.ID))
            {
                validationDictionary.AddError("Entity does not exist", "Entry with given primary key does not exist in the database. There is nothing to update");
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
                validationDictionary.AddError("Word not found", "Word with given primary key does not exist in the database. Please create it before posting an Entry");
                return;
            }

            //one entry per word
            if (repo.ExistsByWord(entity.WordID))
            {
                validationDictionary.AddError("Duplicate", "An Entry for given Word already exists. " +
                    "If you want to add another Meaning, post it on its respective endpoint");
                return;
            }
            
            //check if dictionary exists
            if (!dictRepo.ExistsByIndex(entity.DictionaryIndex))
            {
                validationDictionary.AddError("Dictionary not found", "Dictionary with given Index does not exist in the database. Please create it before posting an Entry");
                return;
            }

            //check if word's source language is dictionary's language in
            var word = wordRepo.GetByID(entity.WordID);
            var dict = dictRepo.GetByIndex(entity.DictionaryIndex);
            if (!String.Equals(word.SourceLanguageName, dict.LanguageInName, StringComparison.OrdinalIgnoreCase))
            {
                validationDictionary.AddError("Language does not match", "SourceLanguage of the Word is different than LanguageIn of the dictionary");
            }

        }
    }
}