using Data.Database;
using Data.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Api.Service
{
    //Every query returning a Word or IEnumerable<Word> should include its properties.
    public class WordRepository : RepositoryBase<Word>, IWordRepository 
    {
        private readonly Func<IQueryable<Word>, IIncludableQueryable<Word, object>> includeQuery = (words => words.Include(w => w.Properties));
        private readonly Func<IQueryable<Word>, IOrderedQueryable<Word>> byLanguageThenByValue = (words => words.OrderBy(w => w.SourceLanguageName).ThenBy(w => w.Value));

        public WordRepository(DatabaseContext context) : base(context) { }

        public override IEnumerable<R> Get<R>(Expression<Func<Word, bool>> condition, Expression<Func<Word, R>> mapper)
        {
            return Get(condition, mapper, byLanguageThenByValue, includeQuery);
        }

        public override Word GetOne(Expression<Func<Word, bool>> condition)
        {
            return GetOne(condition, byLanguageThenByValue, includeQuery);
        }

        public Word GetByID(int id)
        {
            //so we cannot use Find because it is not possible to include children.
            return GetOne(w => w.ID == id, null, includeQuery);
        }

        public IEnumerable<Word> GetByLanguageAndValue(string languageName, string value)
        {
            return Get(w => w.SourceLanguageName == languageName && w.Value == value, x => x,
                null, includeQuery);
        }

        public IEnumerable<Word> GetByValue(string value)
        {
            return Get(w => w.Value == value, x => x,
                words => words.OrderBy(w => w.SourceLanguageName), includeQuery);
        }
    }
}
