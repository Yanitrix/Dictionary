using Domain.Dto;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface IEntryService
    {
        GetEntry Get(int id);

        IEnumerable<GetEntry> GetByDictionaryAndWord(String word, int? dictionaryIndex);

        ValidationResult Add(CreateEntry dto);

        ValidationResult Update(UpdateEntry dto);

        ValidationResult Delete(int id);
    }
}
