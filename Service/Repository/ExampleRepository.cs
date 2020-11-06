using Data.Database;
using Data.Models;
using System.Collections.Generic;

namespace Service.Repository
{
    public class ExampleRepository : RepositoryBase<Example>, IExampleRepository
    {
        public ExampleRepository(DatabaseContext context) : base(context) { }

        public IEnumerable<Example> GetByDictionaryAndTextSubstring(int dictionaryIndex, string textSubstring)
        {
            return Get(e => e.Text.Contains(textSubstring) && e.Meaning.Entry.DictionaryIndex == dictionaryIndex, x => x);
        }

        public IEnumerable<Example> GetByDictionaryAndTranslationSubstring(int dictionaryIndex, string translationSubstring)
        {
            return Get(e => e.Translation.Contains(translationSubstring) && e.Meaning.Entry.DictionaryIndex == dictionaryIndex, x => x);
        }

        public IEnumerable<Example> GetByTextSubstring(string text)
        {
            return Get(e => e.Text.Contains(text), x => x);
        }

        public IEnumerable<Example> GetByTranslationSubstring(string translation)
        {
            return Get(e => e.Translation.Contains(translation), x => x);
        }
    }
}
