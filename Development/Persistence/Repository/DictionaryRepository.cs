using Persistance.Database;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Repository;

namespace Persistance.Repository
{
    public class DictionaryRepository : RepositoryBase<Dictionary>, IDictionaryRepository
    {
        public DictionaryRepository(DatabaseContext context) : base(context) { }

        public IEnumerable<Dictionary> GetAllByLanguage(string languageName)
        {
            return repo.Where(d => d.LanguageInName == languageName || d.LanguageOutName == languageName).ToList();
        }

        public Dictionary GetByIndex(int index)
        {
            return repo.FirstOrDefault(d => d.Index == index);
        }

        public Dictionary GetByLanguageInAndOut(string languageIn, string languageOut)
        {
            return repo.Find(languageIn, languageOut);
        }

        public bool ExistsByIndex(int index)
        {
            return Exists(d => d.Index == index);
        }

        public bool ExistsByLanguages(string langIn, string langOut)
        {
            if (String.IsNullOrEmpty(langIn) || String.IsNullOrWhiteSpace(langIn))
                return false;
            return Exists(d => EF.Functions.Like(d.LanguageInName, langIn)
            && EF.Functions.Like(d.LanguageOutName, langOut));
        }

        public IEnumerable<Dictionary> GetAllByLanguageIn(string langIn)
        {
            if (String.IsNullOrEmpty(langIn) || String.IsNullOrWhiteSpace(langIn))
                return Enumerable.Empty<Dictionary>();
            return Get(d => EF.Functions.Like(d.LanguageInName, langIn), x => x);
        }

        public IEnumerable<Dictionary> GetAllByLanguageOut(string langOut)
        {
            if (String.IsNullOrEmpty(langOut) || String.IsNullOrWhiteSpace(langOut))
                return Enumerable.Empty<Dictionary>();
            return Get(d => EF.Functions.Like(d.LanguageOutName, langOut), x => x);
        }
    }
}
