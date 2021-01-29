using Data.Dto;
using Data.Models;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface ILanguageService : IService<Language>
    {
        IValidationDictionary Delete(String name);

        IEnumerable<LanguageWordCount> AllWithWordCount();

        Language Get(String name);
    }
}
