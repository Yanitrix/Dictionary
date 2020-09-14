using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    public interface IWordService : IService<Word>
    {
        public Word GetByID(int id);

        public IEnumerable<Word> GetByValue(String value);

        public IEnumerable<Word> GetByLanguageAndValue(String languageName, String Value);
    }
}
