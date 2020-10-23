using Data.Database;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Service.Repository
{
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

        //ignores case
        public IEnumerable<Word> GetByLanguageAndValue(String languageName, String value, bool ignoreCase = true)
        {
            if (ignoreCase)
            {
                return Get(w => EF.Functions.Like(w.SourceLanguageName, $"%{languageName}%") && EF.Functions.Like(w.Value, $"%{value}%"),
                    x => x,
                    null, includeQuery);
            }

            return Get(w => w.Value == value && w.SourceLanguageName == languageName, x => x, null, includeQuery);
        }

        public IEnumerable<Word> GetByValue(String value, bool ignoreCase = true)
        {
            if (ignoreCase)
            {
                return Get(w => EF.Functions.Like(w.Value, $"%{value}%"),
                    x => x,
                    words => words.OrderBy(w => w.SourceLanguageName), includeQuery);
            }

            return Get(w => w.Value == value, x => x,
                words => words.OrderBy(w => w.SourceLanguageName), includeQuery);
        }

        public bool ExistsByID(int id)
        {
            return Exists(w => w.ID == id);
        }
    }
}
