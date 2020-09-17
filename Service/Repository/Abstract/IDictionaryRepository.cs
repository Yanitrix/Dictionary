using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    public interface IDictionaryRepository : IRepository<Dictionary>
    {
        IEnumerable<Dictionary> GetAllByLanguage(String languageName);

        Dictionary GetByLanguageInAndOut(String languageIn, String languageOut);

        Dictionary GetByIndex(int index);
    }
}
