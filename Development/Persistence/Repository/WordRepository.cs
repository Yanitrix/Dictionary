using Persistence.Database;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Repository;

namespace Persistence.Repository
{
    public class WordRepository : RepositoryBase<Word, int>, IWordRepository
    {
        private readonly Func<IQueryable<Word>, IIncludableQueryable<Word, object>> includeQuery = (words => words.Include(w => w.Properties));
        private readonly Func<IQueryable<Word>, IOrderedQueryable<Word>> byLanguageThenByValue = (words => words.OrderBy(w => w.SourceLanguageName).ThenBy(w => w.Value));

        public WordRepository(DatabaseContext context) : base(context) { }

        public override IEnumerable<R> Get<R>(Expression<Func<Word, bool>> condition, Expression<Func<Word, R>> mapper)
        {
            return Get(condition, mapper, byLanguageThenByValue, includeQuery);
        }

        public override Word GetByPrimaryKey(int id)
        {
            return GetOne(w => w.ID == id, null, includeQuery);
        }

        public override bool ExistsByPrimaryKey(int id)
        {
            return repo.Any(w => w.ID == id);
        }

        public override Word GetOne(Expression<Func<Word, bool>> condition)
        {
            return GetOne(condition, byLanguageThenByValue, includeQuery);
        }

        //ignores case
        public IEnumerable<Word> GetByLanguageAndValue(String languageName, String value, bool caseSensitive = true)
        {
            if (caseSensitive)
            {
                return Get(w => w.Value == value && w.SourceLanguageName == languageName, x => x, null, includeQuery);
            }

            return Get(
                w =>
                    EF.Functions.Collate(w.SourceLanguageName, "SQL_Latin1_General_CP1_CI_AS") == languageName &&
                    EF.Functions.Collate(w.Value, "SQL_Latin1_General_CP1_CI_AS") == value,
                x => x,
                null,
                includeQuery);
        }

        public IEnumerable<Word> GetByValue(String value, bool caseSensitive = true)
        {
            if (caseSensitive)
            {
                return Get(w => w.Value == value, x => x,
                    words => words.OrderBy(w => w.SourceLanguageName), includeQuery);
            }

            return Get(
                w => EF.Functions.Collate(w.Value, "SQL_Latin1_General_CP1_CI_AS") == value,
                x => x,
                words => words.OrderBy(w => w.SourceLanguageName),
                includeQuery);
        }

        public override void Update(Word entity)
        {
            var inDB = context.Words.Find(entity.ID);
            context.Entry(inDB).Collection(w => w.Properties).Load();
            inDB.Value = entity.Value;
            inDB.Properties = entity.Properties;
            context.SaveChanges();
        }
    }
}
