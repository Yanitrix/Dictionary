﻿using Data.Models;
using System;
using System.Collections.Generic;

namespace Service.Repository
{
    public interface IDictionaryRepository : IRepository<Dictionary>
    {
        IEnumerable<Dictionary> GetAllByLanguage(String languageName);

        Dictionary GetByLanguageInAndOut(String languageIn, String languageOut);

        Dictionary GetByIndex(int index);

        bool ExistsByIndex(int index);

        bool ExistsByLanguages(String languageIn, String languageOut);

    }
}
