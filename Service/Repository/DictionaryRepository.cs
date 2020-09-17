using Data.Database;
using Data.Models;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Api.Service
{
    public class DictionaryRepository : RepositoryBase<Dictionary>, IDictionaryRepository
    {
        public DictionaryRepository(DatabaseContext context) : base(context) { }

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

    }
}
