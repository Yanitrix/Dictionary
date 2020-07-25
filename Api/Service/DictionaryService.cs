using Api.Service.Abstract;
using Dictionary_MVC.Data;
using Dictionary_MVC.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Api.Service
{
    public class DictionaryService : ServiceBase<Dictionary>, IDictionaryService
    {
        public DictionaryService(DatabaseContext context, AbstractValidator<Dictionary> validator):base(context, validator) { }

        public IEnumerable<Dictionary> GetAllByLanguage(string languageName)
        {
            return repo.Where(d => d.LanguageInName == languageName || d.LanguageOutName == languageName);
        }

        public Dictionary GetByIndex(int index)
        {
            return repo.FirstOrDefault(d => d.Index == index);
        }

        public Dictionary GetByLanguageInAndOut(string languageIn, string languageOut)
        {
            return repo.Find(languageIn, languageOut);
        }

        public override bool IsReadyToAdd(Dictionary entity)//check if db already contains entity
        {
            //check if languages are present in the database
            var languageRepo = context.Set<Language>();
            var languagesPresent = languageRepo.Any(lang => lang.Name == entity.LanguageInName) && languageRepo.Any(lang => lang.Name == entity.LanguageOutName);
            if (!languagesPresent) return false;

            //

            return false;
        }

        public override bool IsReadyToUpdate(Dictionary entity)
        {
            return false;
        }

    }
}
