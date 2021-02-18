using Domain.Dto;
using Domain.Models;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface IEntryService
    {
        Entry Get(int id);

        IEnumerable<Entry> GetByDictionaryAndWord(String word, int? dictionaryIndex);

        ValidationResult Add(CreateOrUpdateEntry dto);

        ValidationResult Update(CreateOrUpdateEntry dto);

        ValidationResult Delete(int id);
    }
}
