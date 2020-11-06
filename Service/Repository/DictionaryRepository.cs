using Data.Database;
using Data.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Service.Repository
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

        public bool ExistsByLanguages(string languageIn, string languageOut)
        {
            return Exists(d => EF.Functions.Like(d.LanguageInName, $"%{languageIn}%")
            || EF.Functions.Like(d.LanguageOutName, $"%{languageOut}%"));
        }

        public IEnumerable<Dictionary> GetAllByLanguageIn(string langIn)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Dictionary> GetAllByLanguageOut(string langIn)
        {
            throw new System.NotImplementedException();
        }
    }
}
