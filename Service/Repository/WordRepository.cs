using Data.Database;
using Data.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Service
{
    public class WordRepository : RepositoryBase<Word>, IWordRepository 
    {

        public WordRepository(DatabaseContext context) : base(context) { }

        public Word GetByID(int id)
        {
            return repo.Find(id);
        }

        public IEnumerable<Word> GetByLanguageAndValue(string languageName, string value)
        {
            return repo.Where(w => w.SourceLanguageName == languageName && w.Value == value);
        }

        public IEnumerable<Word> GetByValue(string value)
        {
            return repo.Where(w => w.Value == value);
        }

    }
}
