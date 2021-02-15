using Domain.Models;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface IDictionaryService : IService<Dictionary>
    {
        ValidationResult Delete(int index);

        IEnumerable<Dictionary> GetContainingLanguage(String langIn, String langOut, String language);

        Dictionary Get(int index);
    }
}
