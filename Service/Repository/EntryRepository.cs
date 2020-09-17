using Data.Database;
using Data.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Service
{
    public class EntryRepository : RepositoryBase<Entry>, IEntryRepository
    {
        public EntryRepository(DatabaseContext context):base(context) { }

        public IEnumerable<Entry> GetByDictionaryAndWord(int dictionaryIndex, string wordValue)
        {
            return repo.Where(entry => entry.DictionaryIndex == dictionaryIndex && entry.Word.Value == wordValue);
        }

        public Entry GetByID(int id)
        {
            return repo.Find(id);
        }

    }
}
