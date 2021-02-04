using Data.Database;
using Data.Models;
using Data.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repository
{
    public class FreeExpressionRepository : ExpressionRepositoryBase<FreeExpression>, IFreeExpressionRepository
    {
        public FreeExpressionRepository(DatabaseContext context) : base(context) { }

        public override IEnumerable<FreeExpression> GetByDictionaryAndTextSubstring(int dictionaryIndex, string textSubstring)
        {
            if (String.IsNullOrEmpty(textSubstring) || String.IsNullOrWhiteSpace(textSubstring)) return Enumerable.Empty<FreeExpression>();
            return Get(e => e.Text.Contains(textSubstring) && e.DictionaryIndex == dictionaryIndex,
                x => x, orderFunction);
        }

        public override IEnumerable<FreeExpression> GetByDictionaryAndTranslationSubstring(int dictionaryIndex, string translationSubstring)
        {
            if (String.IsNullOrEmpty(translationSubstring) || String.IsNullOrWhiteSpace(translationSubstring)) return Enumerable.Empty<FreeExpression>();
            return Get(e => e.Translation.Contains(translationSubstring) && e.DictionaryIndex == dictionaryIndex,
                x => x, orderFunction);
        }
    }
}
