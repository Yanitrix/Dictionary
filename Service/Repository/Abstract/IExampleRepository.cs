using Data.Models;
using System;
using System.Collections.Generic;

namespace Service.Repository
{
    public interface IExampleRepository : IRepository<Example>
    {
        public IEnumerable<Example> GetByTextSubstring(String text);

        public IEnumerable<Example> GetByTranslationSubstring(String translation);

        /// <summary>
        /// Retrieves a collection of Expressions with similar Text and given Dictionary.Index
        /// </summary>
        /// <param name="dictionaryIndex"></param>
        /// <param name="textSubstring"></param>
        public IEnumerable<Example> GetByDictionaryAndTextSubstring(int dictionaryIndex, string textSubstring);

        /// <summary>
        /// Retrieves a collection of Expressions with similar Translation and given Dictionary.Index
        /// </summary>
        /// <param name="dictionaryIndex"></param>
        /// <param name="translationSubstring"></param>
        public IEnumerable<Example> GetByDictionaryAndTranslationSubstring(int dictionaryIndex, string translationSubstring);
    }
}
