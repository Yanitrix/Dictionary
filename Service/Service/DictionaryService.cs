using Data.Database;
using Data.Models;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Api.Service
{
    public class DictionaryService : ServiceBase<Dictionary>, IDictionaryService
    {
        public DictionaryService(DatabaseContext context, AbstractValidator<Dictionary> validator) : base(context, validator) { }

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

        public override Dictionary Delete(Dictionary entity) //FreeExpressions must be included
        {
            context.Set<Expression>().RemoveRange(entity.FreeExpressions);
            base.Delete(entity);
            return entity;
        }

        //TODO check logic once again when im sober
        public override bool IsReadyToAdd(Dictionary entity)
        {
            if (!IsValid(entity)) return false;

            var indb = repo.Any(d => d.LanguageInName == entity.LanguageInName && d.LanguageOutName == entity.LanguageOutName);
            if (indb)
            {
                ValidationDictionary.AddError("Duplicate", $"A dictionary entity with LanguageIn: {entity.LanguageInName} " +
                    $"and LanguageOut: {entity.LanguageOutName} already exsists");
                return false;
            }

            //check if languages are present in the database
            var languageRepo = context.Set<Language>();
            var languagesPresent = languageRepo.Any(lang => lang.Name == entity.LanguageInName) && languageRepo.Any(lang => lang.Name == entity.LanguageOutName);
            if (!languagesPresent)
            {
                ValidationDictionary.AddError("Language not found", $"LanguageIn \"{entity.LanguageInName}\" or LanguageOut \"{entity.LanguageOutName}\" " +
                    $"doesn't exist in the database. Please create them before posting a dictionary");
                return false;
            }

            return true;
        }

        public override bool IsReadyToUpdate(Dictionary entity)
        {
            //  since LanguageIn and LanguageOut cannot be updated, there's nothing to update in a dictionary. All One-to-Many relationships are 
            //  updated on the child's side

            return false;
        }

    }
}
