using Data.Models;
using System;
using System.Collections.Generic;

namespace Service.Repository
{
    public interface IExpressionRepository : IRepository<Expression>
    {
        public Expression GetByID(int id);

        public IEnumerable<Expression> GetByTextSubstring(String text);

        public IEnumerable<Expression> GetByTranslationSubstring(String translation);

        /// <summary>
        /// Retrieves a collection of Expressions with similar Text and given Dictionary.Index
        /// </summary>
        /// <param name="dictionaryIndex"></param>
        /// <param name="textSubstring"></param>
        public IEnumerable<Expression> GetByDictionaryTextSubstring(int dictionaryIndex, string textSubstring);

        /// <summary>
        /// Retrieves a collection of Expressions with similar Translation and given Dictionary.Index
        /// </summary>
        /// <param name="dictionaryIndex"></param>
        /// <param name="translationSubstring"></param>
        public IEnumerable<Expression> GetByDictionaryTranslationSubstring(int dictionaryIndex, string translationSubstring);
    }
}
