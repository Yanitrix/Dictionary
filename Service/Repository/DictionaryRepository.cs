using Data.Database;
using Data.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
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

        public override Dictionary Delete(Dictionary entity)
        {
            var children = context.Expressions.Where(e => e.DictionaryIndex == entity.Index);
            context.Expressions.RemoveRange(children);
            //context.Entry(entity).State = EntityState.Deleted;
            //context.Entry(entity).Collection(d => d.FreeExpressions).Load();
            //context.Set<Expression>().RemoveRange(entity.FreeExpressions);
            repo.Remove(entity);
            context.SaveChanges();
            return entity;
        }

        public bool ExistsByIndex(int index)
        {
            return Exists(d => d.Index == index);
        }

        public bool ExistsByLanguages(string languageIn, string languageOut)
        {
            return Exists(d => EF.Functions.Like(d.LanguageInName, $"%{languageIn}%")
            || EF.Functions.Like(d.LanguageOutName, $"%{languageOut}%"));
        }
    }
}
