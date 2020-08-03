using Dictionary_MVC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service.Abstract
{
    public interface IWordService : IService<Word>
    {
        public Word GetByID(int id);

        public IEnumerable<Word> GetByValue(String value);

        public IEnumerable<Word> GetByLanguageAndSpeechPartAndValue(String languageName, String speechPartName, String value);

        public IEnumerable<Word> GetByLanguageAndSpeechPart(String languageName, String speechPartName);
    }
}
