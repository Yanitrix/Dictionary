using Data.Dto;
using Data.Models;
using System;
using System.Collections.Generic;

namespace Service.Repository
{
    public interface ILanguageRepository : IRepository<Language>
    {
        IEnumerable<LanguageWordCount> AllWithWordCount();

        public Language GetByNameWithWords(String name);

        Language GetByName(String name);

        bool ExistsByName(String name);
    }
}
