using Domain.Models;
using System;
using System.Collections.Generic;

namespace Domain.Repository
{
    public interface IExpressionRepositoryBase<T> : IRepository<T, int> where T : Expression
    {
        public T GetByID(int id);

        public IEnumerable<T> GetByTextSubstring(String text);

        public IEnumerable<T> GetByTranslationSubstring(String translation);

        /// <summary>
        /// Retrieves a collection of Expressions with similar <b>Text</b> and belonging to a Dictionary with given <b>dictionaryIndex</b>
        /// </summary>
        /// <param name="dictionaryIndex"></param>
        /// <param name="textSubstring"></param>
        public IEnumerable<T> GetByDictionaryAndTextSubstring(int dictionaryIndex, string textSubstring);

        /// <summary>
        /// Retrieves a collection of Expressions with similar <b>Translation</b> and belonging to a Dictionary with given <b>dictionaryIndex</b>
        /// </summary>
        /// <param name="dictionaryIndex"></param>
        /// <param name="translationSubstring"></param>
        public IEnumerable<T> GetByDictionaryAndTranslationSubstring(int dictionaryIndex, string translationSubstring);
    }
}
