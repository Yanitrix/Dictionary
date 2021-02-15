﻿using Domain.Models;
using Domain.Repository;
using System;
using System.Collections.Generic;
using Msg = Service.ValidationErrorMessages;

namespace Service
{
    public class EntryService : IEntryService
    {
        private readonly IWordRepository wordRepo;
        private readonly IDictionaryRepository dictRepo;
        private readonly IEntryRepository repo;
        private ValidationResult validationDictionary;

        public EntryService(IUnitOfWork uow)
        {
            this.wordRepo = uow.Words;
            this.dictRepo = uow.Dictionaries;
            this.repo = uow.Entries;
        }

        public Entry Get(int id) => repo.GetByID(id);
        
        //todo test it
        public IEnumerable<Entry> GetByDictionaryAndWord(String word, int? dictionaryIndex)
        {
            if (word == null && dictionaryIndex == null)
                return Array.Empty<Entry>();
            if (word != null && dictionaryIndex != null)
                return repo.GetByDictionaryAndWord(dictionaryIndex.Value, word, false);
            if (word != null)
                return repo.GetByWord(word, false);
            return repo.GetByDictionary(dictionaryIndex.Value);
        }

        public ValidationResult Add(Entry entity)
        {
            this.validationDictionary = ValidationResult.New(entity);

            CheckAll(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);

            return validationDictionary;
        }

        public ValidationResult Update(Entry entity)
        {
            this.validationDictionary = ValidationResult.New(entity);

            //check if exists
            if (!repo.ExistsByID(entity.ID))
            {
                validationDictionary.AddError(Msg.EntityNotFound(), Msg.ThereIsNothingToUpdate<Entry>());
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

        public ValidationResult Delete(int id)
        {
            var result = ValidationResult.New(id);

            var indb = repo.GetByID(id);

            if (indb == null)
            {
                result.AddError(Msg.EntityNotFound<Entry>(), Msg.EntityDoesNotExistByPrimaryKey<Entry>());
            }
            else
            {
                repo.Delete(indb);
            }

            return result;
        }

        private void CheckAll(Entry entity)
        {
            //check if word exists
            if (!wordRepo.ExistsByID(entity.WordID))
            {
                validationDictionary.AddError(Msg.EntityNotFound<Word>(), Msg.EntityDoesNotExistByForeignKey<Entry, Word>(w => w.ID, entity.WordID));
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
                validationDictionary.AddError(Msg.EntityNotFound<Dictionary>(),
                    Msg.EntityDoesNotExistByForeignKey<Entry, Dictionary>(d => d.Index, entity.DictionaryIndex));
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