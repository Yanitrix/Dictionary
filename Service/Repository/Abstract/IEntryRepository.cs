using Data.Models;
using System;
using System.Collections.Generic;

namespace Service.Repository
{
    public interface IEntryRepository : IRepository<Entry>
    {
        public Entry GetByID(int id);
        /// <summary>
        /// Including Word, Meanings and their examples
        /// </summary>
        /// <param name="dictionaryIndex"></param>
        /// <param name="wordValue"></param>
        /// <returns>Matching collection</returns>
        public IEnumerable<Entry> GetByDictionaryAndWord(int dictionaryIndex, String wordValue);

        public bool ExistsByID(int id);

        /// <summary>
        /// Checks if theres already another Entry that contains Word of given ID
        /// </summary>
        /// <param name="wordID"></param>
        /// <returns>a boolean</returns>
        public bool ExistsByWord(int wordID);
    }
}
