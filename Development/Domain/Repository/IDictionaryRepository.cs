using Domain.Models;
using System;
using System.Collections.Generic;

namespace Domain.Repository
{
    public interface IDictionaryRepository : IRepository<Dictionary, int>
    {
        IEnumerable<Dictionary> GetAllByLanguage(String languageName);

        Dictionary GetByLanguageInAndOut(String languageIn, String languageOut);

        bool ExistsByLanguages(String languageIn, String languageOut);

        IEnumerable<Dictionary> GetAllByLanguageIn(string langIn);
        
        IEnumerable<Dictionary> GetAllByLanguageOut(string langIn);
    }
}
