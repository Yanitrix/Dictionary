using Data.Models;
using System;
using System.Collections.Generic;

namespace Service.Repository
{
    public interface IEntryRepository : IRepository<Entry>
    {
        public Entry GetByID(int id);

        /// <summary>
        /// Including Word and its Properties, Meanings and their Examples, Dictionary.
        /// <i>wordValue</i> is case insensitive
        /// </summary>
        /// <param name="dictionaryIndex"></param>
        /// <param name="wordValue"></param>
        /// <returns>Matching collection</returns>
        public IEnumerable<Entry> GetByDictionaryAndWord(int dictionaryIndex, String wordValue);

        /// <summary>
        /// Including Word and its Properties, Meanings and their Examples, Dictionary
        /// <i>wordValue</i> is case insensitive
        /// </summary>
        /// <param name="wordValue"></param>
        /// <returns></returns>
        public IEnumerable<Entry> GetByWord(String wordValue);

        /// <summary>
        /// Returns all Entries owned by certain Dictionary
        /// Including Word and its Properties, Meanings and their Examples, Dictionary
        /// </summary>
        /// <param name="dictionaryIndex"></param>
        /// <returns></returns>
        public IEnumerable<Entry> GetByDictionary(int dictionaryIndex);

        public bool ExistsByID(int id);

        /// <summary>
        /// Checks if theres already another Entry that contains Word of given ID
        /// </summary>
        /// <param name="wordID"></param>
        /// <returns>a boolean</returns>
        public bool ExistsByWord(int wordID);

        public bool HasMeanings(int id);
    }
}
