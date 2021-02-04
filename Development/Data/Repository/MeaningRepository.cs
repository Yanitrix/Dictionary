using Data.Database;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repository
{
    public class MeaningRepository : RepositoryBase<Meaning>, IMeaningRepository
    {
        //All queries loaded with Examples
        public MeaningRepository(DatabaseContext context):base(context) { }

        private readonly Func<IQueryable<Meaning>, IIncludableQueryable<Meaning, object>> includeExamples =
            (meanings => meanings.Include(m => m.Examples));
        private readonly Func<IQueryable<Meaning>, IIncludableQueryable<Meaning, object>> includeExamplesAndEntry =
            (meanings => meanings.Include(m => m.Examples).Include(m => m.Entry));

        public Meaning GetByID(int id)
        {
            return GetOne(m => m.ID == id, null, includeExamples);
        }

        public Meaning GetByIDWithEntry(int id)
        {
            return GetOne(m => m.ID == id, null, includeExamplesAndEntry);
        }

        //includes Entry
        public IEnumerable<Meaning> GetByValueSubstring(string value)
        {
            if (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value)) return Enumerable.Empty<Meaning>();
            return Get(m => m.Value.Contains(value), x => x, null, includeExamplesAndEntry);
        }

        //includes Entry
        public IEnumerable<Meaning> GetByNotesSubstring(string notes)
        {
            if (String.IsNullOrEmpty(notes) || String.IsNullOrWhiteSpace(notes)) return Enumerable.Empty<Meaning>();
            return Get(m => m.Notes.Contains(notes), x => x, null, includeExamplesAndEntry);
        }

        public bool ExistsByID(int id)
        {
            return Exists(m => m.ID == id);
        }

        public IEnumerable<Meaning> GetByDictionaryAndValueSubstring(int dictionaryIndex, string valueSubstring)
        {
            if (String.IsNullOrEmpty(valueSubstring))
                return Enumerable.Empty<Meaning>();
            return Get(m => m.Entry.DictionaryIndex == dictionaryIndex && EF.Functions.Like(m.Value, $"%{valueSubstring}%"), x => x);
        }
        //Does not update EntryID
        public override void Update(Meaning entity)
        {
            //retrieve, change, update
            var inDB = context.Meanings.Find(entity.ID);
            context.Entry(inDB).Collection(m => m.Examples).Load();
            inDB.Value = entity.Value;
            inDB.Notes = entity.Notes;
            inDB.Examples = entity.Examples;
            context.SaveChanges();
        }
    }
}
