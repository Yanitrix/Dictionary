using Data.Database;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using Data.Dto;

namespace Service.Repository
{
    
    public class LanguageRepository : RepositoryBase<Language>, ILanguageRepository
    {
        private readonly Func<IQueryable<Language>, IIncludableQueryable<Language, object>> includeQuery = 
            (languages => languages.Include(l => l.Words).ThenInclude(w => w.Properties));

        public LanguageRepository(DatabaseContext context):base(context) { }

        public override void Delete(Language entity)
        {
            var children = context.Set<Dictionary>().Where(d => d.LanguageInName == entity.Name || d.LanguageOutName == entity.Name);
            context.RemoveRange(children);
            base.Delete(entity);
        }

        public Language GetByName(String name)
        {
            return repo.Find(name);
        }

        public Language GetByNameWithWords(String name)
        {
            return GetOne(l => l.Name == name, null, includeQuery);
        }

        public Language GetOneWithWords(Expression<Func<Language, bool>> condition)
        {
            return GetOne(condition, null, includeQuery);
        }

        public override void Create(Language entity)
        {
            entity.Name = entity.Name.ToLower();
            base.Create(entity);
        }

        public bool ExistsByName(string name)
        {
            return Exists(l => EF.Functions.Like(l.Name, $"{name}"));
        }

        public IEnumerable<LanguageWordCount> AllWithWordCount()
        {
            return repo.Select(lang => new LanguageWordCount { Name = lang.Name, WordCount = lang.Words.Count }).OrderBy(count => count.Name).ToList();
        }
    }
}
