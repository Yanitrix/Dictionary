using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    public interface IWordRepository : IRepository<Word>
    {
        public Word GetByID(int id);

        public IEnumerable<Word> GetByValue(String value, bool ignoreCase);

        public IEnumerable<Word> GetByLanguageAndValue(String languageName, String Value);

        public bool ExistsByID(int id);
    }
}
