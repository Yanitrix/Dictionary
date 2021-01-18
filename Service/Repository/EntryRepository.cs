using Data.Database;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Repository
{
    public class EntryRepository : RepositoryBase<Entry>, IEntryRepository
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

        //ignores case
        public IEnumerable<Entry> GetByDictionaryAndWord(int dictionaryIndex, string wordValue)
        {
            if (String.IsNullOrEmpty(wordValue) || String.IsNullOrWhiteSpace(wordValue)) return Enumerable.Empty<Entry>();
            return Get(e => e.DictionaryIndex == dictionaryIndex && EF.Functions.Like(e.Word.Value, $"{wordValue}"),
                x => x,
                orderFunction,
                includeQuery);
        }

        //ignores case
        public IEnumerable<Entry> GetByWord(string wordValue)
        {
            if (String.IsNullOrEmpty(wordValue) || String.IsNullOrWhiteSpace(wordValue)) return Enumerable.Empty<Entry>();
            return Get(e => EF.Functions.Like(e.Word.Value, $"{wordValue}"),
                x => x,
                orderFunction,
                includeQuery);
        }

        public IEnumerable<Entry> GetByDictionary(int dictionaryIndex)
        {
            return Get(e => e.DictionaryIndex == dictionaryIndex,
                x => x, orderFunction, includeQuery);
        }
        
        public Entry GetByID(int id)
        {
            return GetOne(e => e.ID == id, null, includeQuery);
        }

        public bool ExistsByID(int id)
        {
            return Exists(e => e.ID == id);
        }

        public bool ExistsByWord(int wordID)
        {
            return Exists(e => e.WordID == wordID);
        }

        public bool HasMeanings(int id)
        {
            return context.Meanings.Any(m => m.EntryID == id);
        }
    }
}
