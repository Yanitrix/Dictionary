using Domain.Dto;
using Domain.Models;
using System;
using System.Collections.Generic;

namespace Domain.Repository
{
    public interface ILanguageRepository : IRepository<Language, String>
    {
        IEnumerable<LanguageWordCount> AllWithWordCount();

        Language GetByNameWithWords(String name);

    }
}
