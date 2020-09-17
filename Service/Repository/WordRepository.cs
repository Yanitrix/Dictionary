using Data.Database;
using Data.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Service
{
    public class WordRepository : RepositoryBase<Word>, IWordRepository 
    {

        public WordRepository(DatabaseContext context, AbstractValidator<Word> validator) : base(context, validator) { }

        public Word GetByID(int id)
        {
            return repo.Find(id);
        }

        public IEnumerable<Word> GetByLanguageAndValue(string languageName, string value)
        {
            return repo.Where(w => w.SourceLanguageName == languageName && w.Value == value);
        }

        public IEnumerable<Word> GetByValue(string value)
        {
            return repo.Where(w => w.Value == value);
        }

        public override bool IsReadyToAdd(Word entity)
        {
            if (!IsValid(entity)) return false;

            //check if language exists in db
            var langindb = context.Set<Language>().Find(entity.SourceLanguageName);
            if (langindb == null)
            {
                ValidationDictionary.AddError("Source language not found", "Language by the name " +
                    $"of \"{entity.SourceLanguageName}\" doesn't exist in the database");
                return false;
            }

            //correctness of word properties will be checked in WordPropertyService

            return true;
        }

        public override bool IsReadyToUpdate(Word entity)
        {
            if (!IsValid(entity)) return false;

            //there aren't any different assumptions when adding vs when updating, so the same will be done.

            return IsReadyToAdd(entity);
        }
    }
}
