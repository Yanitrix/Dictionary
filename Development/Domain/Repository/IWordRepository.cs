using Domain.Models;
using System;
using System.Collections.Generic;

namespace Domain.Repository
{
    /// <summary>
    /// Every query includes Properties
    /// </summary>
    public interface IWordRepository : IRepository<Word>
    {
        public Word GetByID(int id);

        /// <summary>
        /// Ignores case by default.
        /// </summary>
        /// <returns>Matching collection</returns>
        public IEnumerable<Word> GetByValue(String value, bool caseSensitive = true);

        /// <summary>
        /// Ignores case by default
        /// </summary>
        /// <returns>Matching collection</returns>
        public IEnumerable<Word> GetByLanguageAndValue(String languageName, String Value, bool ignoreCase = true);

        public bool ExistsByID(int id);
    }
}
