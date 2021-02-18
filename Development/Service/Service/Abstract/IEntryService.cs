using Domain.Dto;
using Domain.Dto.Entry;
using Domain.Models;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface IEntryService
    {
        Entry Get(int id);

        IEnumerable<Entry> GetByDictionaryAndWord(String word, int? dictionaryIndex);

        ValidationResult Add(CreateEntry dto);

        ValidationResult Update(UpdateEntry dto);

        ValidationResult Delete(int id);
    }
}
