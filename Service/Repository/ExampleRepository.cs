using Data.Database;
using Data.Models;
using Service.Repository.Abstract.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Repository
{
    public class ExampleRepository : ExpressionRepositoryBase<Example>, IExampleRepository
    {
        public ExampleRepository(DatabaseContext context) : base(context) { }

        public override IEnumerable<Example> GetByDictionaryAndTextSubstring(int dictionaryIndex, string textSubstring)
        {
            if (String.IsNullOrEmpty(textSubstring) || String.IsNullOrWhiteSpace(textSubstring)) return Enumerable.Empty<Example>();
            return Get(e => e.Text.Contains(textSubstring) && e.Meaning.Entry.DictionaryIndex == dictionaryIndex,
                x => x, orderFunction);
        }

        public override IEnumerable<Example> GetByDictionaryAndTranslationSubstring(int dictionaryIndex, string translationSubstring)
        {
            if (String.IsNullOrEmpty(translationSubstring) || String.IsNullOrWhiteSpace(translationSubstring)) return Enumerable.Empty<Example>();
            return Get(e => e.Translation.Contains(translationSubstring) && e.Meaning.Entry.DictionaryIndex == dictionaryIndex,
                x => x, orderFunction);
        }
    }
}
