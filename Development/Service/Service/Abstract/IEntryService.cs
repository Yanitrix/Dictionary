using Domain.Models;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface IEntryService : IService<Entry>
    {
        Entry Get(int id);

        ValidationResult Delete(int id);

        public IEnumerable<Entry> GetByDictionaryAndWord(String word, int? dictionaryIndex);
    }
}
