using Data.Models;
using System;
using System.Collections.Generic;

namespace Service.Repository
{
    public interface IFreeExpressionRepository : IRepository<FreeExpression>
    {
        public FreeExpression GetByID(int id);

        //TODO write about code duplication.
        public IEnumerable<FreeExpression> GetByTextSubstring(String text);

        public IEnumerable<FreeExpression> GetByTranslationSubstring(String translation);

        /// <summary>
        /// Retrieves a collection of Expressions with similar Text and given Dictionary.Index
        /// </summary>
        /// <param name="dictionaryIndex"></param>
        /// <param name="textSubstring"></param>
        public IEnumerable<FreeExpression> GetByDictionaryAndTextSubstring(int dictionaryIndex, string textSubstring);

        /// <summary>
        /// Retrieves a collection of Expressions with similar Translation and given Dictionary.Index
        /// </summary>
        /// <param name="dictionaryIndex"></param>
        /// <param name="translationSubstring"></param>
        public IEnumerable<FreeExpression> GetByDictionaryAndTranslationSubstring(int dictionaryIndex, string translationSubstring);
    }
}
