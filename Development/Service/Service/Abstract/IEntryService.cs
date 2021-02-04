using Data.Models;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface IEntryService : IService<Entry>
    {
        Entry Get(int id);

        IValidationDictionary Delete(int id);

        public IEnumerable<Entry> GetByDictionaryAndWord(String word, int? dictionaryIndex);
    }
}
