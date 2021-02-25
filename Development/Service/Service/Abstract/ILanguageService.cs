using Domain.Dto;
using Domain.Models;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface ILanguageService
    {
        Language Get(String name);

        IEnumerable<LanguageWordCount> AllWithWordCount();

        ValidationResult Add(CreateLanguage dto);

        ValidationResult Delete(String name);
    }
}
