using Api.Service.Abstract;
using Api.Validation;
using Dictionary_MVC.Data;
using Dictionary_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Service
{
    public class WordService : ServiceBase<Word>, IWordService 
    {

        public WordService(DatabaseContext context, WordValidator validator) : base(context, validator) { }

        public Word GetByID(int id)
        {
            return repo.Find(id);
        }

        public IEnumerable<Word> GetByLanguageAndSpeechPart(string languageName, string speechPartName)
        {
            return repo.Where(w => w.SourceLanguageName == languageName && w.SpeechPartName == speechPartName);
        }

        public IEnumerable<Word> GetByLanguageAndSpeechPartAndValue(string languageName, string speechPartName, string value)
        {
            return repo.Where(w => w.SourceLanguageName == languageName && w.SpeechPartName == speechPartName && w.Value == value);
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

            //check if speech part exists in the db
            var speechpartinDB = context.Set<SpeechPart>().Find(entity.SourceLanguageName, entity.SpeechPartName);
            if (speechpartinDB == null)
            {
                ValidationDictionary.AddError("Speech part not found", "Speech part by the name of \"{entity.SpeechPartName}\" " +
                    $"and with language \"{entity.SourceLanguageName}\" doesn't exist in the database");
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
