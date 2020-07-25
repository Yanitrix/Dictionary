using Dictionary_MVC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service.Abstract
{
    public interface IDictionaryService : IService<Dictionary>
    {
        IEnumerable<Dictionary> GetAllByLanguage(String languageName);

        Dictionary GetByLanguageInAndOut(String languageIn, String languageOut);

        Dictionary GetByIndex(int index);
    }
}
