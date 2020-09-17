using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    public interface IEntryRepository : IRepository<Entry>
    {
        public Entry GetByID(int id);

        public IEnumerable<Entry> GetByDictionaryAndWord(int dictionaryIndex, String wordValue);
    }
}
