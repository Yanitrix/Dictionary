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
    public class EntryRepository : RepositoryBase<Entry, int>, IEntryRepository
    {
        public EntryRepository(DatabaseContext context):base(context) { }

        private readonly Func<IQueryable<Entry>, IOrderedQueryable<Entry>> orderFunction =
            src => src.OrderBy(e => e.DictionaryIndex).ThenBy(e => e.Word.Value);
        //include everything
        private readonly Func<IQueryable<Entry>, IIncludableQueryable<Entry, object>> includeQuery =
            entries => entries
            .Include(e => e.Dictionary)
            .Include(e => e.Word).ThenInclude(w => w.Properties)
            .Include(e => e.Meanings).ThenInclude(m => m.Examples);

        public override IEnumerable<R> Get<R>(Expression<Func<Entry, bool>> condition, Expression<Func<Entry, R>> mapper)
        {
            return base.Get(condition, mapper, orderFunction, includeQuery);
        }

        public override bool ExistsByPrimaryKey(int id)
        {
            return Exists(e => e.ID == id);
        }

        public override Entry GetByPrimaryKey(int id)
        {
            return GetOne(e => e.ID == id, null, includeQuery);
        }

        public IEnumerable<Entry> GetByDictionaryAndWord(int dictionaryIndex, string wordValue, bool caseSensitive = true)
        {
            if (String.IsNullOrEmpty(wordValue) || String.IsNullOrWhiteSpace(wordValue)) return Enumerable.Empty<Entry>();

            if(caseSensitive)
            {
                return Get(
                    e => e.DictionaryIndex == dictionaryIndex && e.Word.Value == wordValue,
                    x => x, orderFunction, includeQuery);
            }

            return Get(
                e => e.DictionaryIndex == dictionaryIndex &&
                    EF.Functions.Collate(e.Word.Value, "SQL_Latin1_General_CP1_CI_AS") == wordValue,
                x => x, orderFunction, includeQuery
                );
        }

        public IEnumerable<Entry> GetByWord(string wordValue, bool caseSensitive = true)
        {
            if (String.IsNullOrEmpty(wordValue) || String.IsNullOrWhiteSpace(wordValue)) return Enumerable.Empty<Entry>();
            if (caseSensitive)
            {
                return Get(e => EF.Functions.Like(e.Word.Value, $"{wordValue}"),
                    x => x,
                    orderFunction,
                    includeQuery);
            }

            return Get(
                e => EF.Functions.Collate(e.Word.Value, "SQL_Latin1_General_CP1_CI_AS") == wordValue,
                x => x, orderFunction, includeQuery
                );
        }

        public IEnumerable<Entry> GetByDictionary(int dictionaryIndex)
        {
            return Get(e => e.DictionaryIndex == dictionaryIndex,
                x => x, orderFunction, includeQuery);
        }

        public bool HasMeanings(int id)
        {
            return context.Meanings.Any(m => m.EntryID == id);
        }
    }
}
