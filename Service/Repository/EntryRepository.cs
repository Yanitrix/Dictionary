﻿using Data.Database;
using Data.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Service
{
    public class EntryRepository : RepositoryBase<Entry>, IEntryRepository
    {
        public EntryRepository(DatabaseContext context, AbstractValidator<Entry> validator):base(context, validator) { }

        public IEnumerable<Entry> GetByDictionaryAndWord(int dictionaryIndex, string wordValue)
        {
            return repo.Where(entry => entry.DictionaryIndex == dictionaryIndex && entry.Word.Value == wordValue);
        }

        public Entry GetByID(int id)
        {
            return repo.Find(id);
        }

        public override bool IsReadyToAdd(Entry entity)
        {
            if (!IsValid(entity)) return false;

            //check if there's another Entry with the same WordID

            if (repo.Any(e => e.WordID == entity.WordID))
            {
                ValidationDictionary.AddError("Word already in use", "There already is an Entry with given " +
                    "WordID. One Entry can be only bound to one Word");
                return false;
            }

            //check if dictionary exists
            var dictionaryIndb = context.Set<Dictionary>().FirstOrDefault(d => d.Index == entity.DictionaryIndex);
            if (dictionaryIndb == null)
            {
                ValidationDictionary.AddError("Dictionary not found", "Dictionary with " +
                    $"index \"{entity.DictionaryIndex}\" doesn't exist in the database");
                return false;
            }

            //check if word exists
            var wordIndb = context.Set<Word>().Find(entity.WordID);
            if (wordIndb == null)
            {
                ValidationDictionary.AddError("Word not found", "Word with " +
                    $"ID \"{entity.WordID}\" doesn't exist in the database");
                return false;
            }

            //check if word's language and dictionary's language is the same
            if (dictionaryIndb.LanguageInName != wordIndb.SourceLanguageName)
            {
                ValidationDictionary.AddError("The source language of the word and the input language of the dictionary are't equal.",
                    $"Word's language is {wordIndb.SourceLanguageName} whereas input language of the dictionary is {dictionaryIndb.LanguageInName}. " +
                    $"This entry should be added to a dictionary with appropriate input language");
                return false;
            }

            return true;
        }

        public override bool IsReadyToUpdate(Entry entity)
        {
            //no extra things to check
            return IsReadyToAdd(entity);
        }
    }
}