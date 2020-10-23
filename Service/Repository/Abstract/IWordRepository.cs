using Data.Models;
using System;
using System.Collections.Generic;

namespace Service.Repository
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
        public IEnumerable<Word> GetByValue(String value, bool ignoreCase);

        /// <summary>
        /// Ignores case by default
        /// </summary>
        /// <returns>Matching collection</returns>
        public IEnumerable<Word> GetByLanguageAndValue(String languageName, String Value, bool ignoreCase = true);

        public bool ExistsByID(int id);
    }
}
