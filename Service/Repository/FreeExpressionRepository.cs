using Data.Database;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Repository
{
    public class FreeExpressionRepository : RepositoryBase<FreeExpression>, IFreeExpressionRepository
    {
        public FreeExpressionRepository(DatabaseContext context) : base(context) { }

        public IEnumerable<FreeExpression> GetByDictionaryAndTextSubstring(int dictionaryIndex, string textSubstring)
        {
            if (textSubstring == null || String.IsNullOrWhiteSpace(textSubstring)) return Enumerable.Empty<FreeExpression>();
            return Get(f => f.Text.Contains(textSubstring) && f.DictionaryIndex == dictionaryIndex, x => x);
        }

        public IEnumerable<FreeExpression> GetByDictionaryAndTranslationSubstring(int dictionaryIndex, string translationSubstring)
        {
            if (translationSubstring == null || String.IsNullOrWhiteSpace(translationSubstring)) return Enumerable.Empty<FreeExpression>();
            return Get(f => f.Translation.Contains(translationSubstring) && f.DictionaryIndex == dictionaryIndex, x => x);
        }

        public IEnumerable<FreeExpression> GetByTextSubstring(string text)
        {
            if (text == null || String.IsNullOrWhiteSpace(text)) return Enumerable.Empty<FreeExpression>();
            return Get(f => f.Text.Contains(text), x => x);
        }

        public IEnumerable<FreeExpression> GetByTranslationSubstring(string translation)
        {
            if (translation == null || String.IsNullOrWhiteSpace(translation)) return Enumerable.Empty<FreeExpression>();
            return Get(f => f.Translation.Contains(translation), x => x);
        }
    }
}
