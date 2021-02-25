using Domain.Dto;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface ILanguageService
    {
        GetLanguage Get(String name);

        IEnumerable<LanguageWordCount> AllWithWordCount();

        ValidationResult Add(CreateLanguage dto);

        ValidationResult Delete(String name);
    }
}
