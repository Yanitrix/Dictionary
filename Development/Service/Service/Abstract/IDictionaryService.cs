using Domain.Dto;
using Domain.Models;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface IDictionaryService
    {
        Dictionary Get(int index);

        IEnumerable<Dictionary> GetContainingLanguage(String langIn, String langOut, String language);

        ValidationResult Add(CreateDictionary dto);

        ValidationResult Delete(int index);
    }
}
