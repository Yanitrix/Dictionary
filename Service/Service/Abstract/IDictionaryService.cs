using Data.Models;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface IDictionaryService : IService<Dictionary>
    {
        IValidationDictionary Delete(int index);

        IEnumerable<Dictionary> GetContainingLanguage(String langIn, String langOut, String language);

        Dictionary Get(int index);
    }
}
