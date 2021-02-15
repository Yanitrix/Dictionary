using Persistance.Database;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using Domain.Dto;
using Domain.Repository;

namespace Persistance.Repository
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
            return repo.FirstOrDefault(l => l.Name == name);
        }

        public Language GetByNameWithWords(String name)
        {
            return GetOne(l => l.Name == name, null, includeQuery);
        }

        //unused method, should be deleted
        public Language GetOneWithWords(Expression<Func<Language, bool>> condition)
        {
            return GetOne(condition, null, includeQuery);
        }

        public override void Create(Language entity)
        {
            entity.Name = entity.Name.ToLower();
            base.Create(entity);
        }

        //case sensitive
        public bool ExistsByName(String name)
        {
            return Exists(l => l.Name == name);
        }

        public IEnumerable<LanguageWordCount> AllWithWordCount()
        {
            return repo.Select(lang => new LanguageWordCount { Name = lang.Name, WordCount = lang.Words.Count }).OrderBy(count => count.Name).ToList();
        }
    }
}
