using Data.Models;
using System;
using System.Collections.Generic;

namespace Service.Repository
{
    public interface IEntryRepository : IRepository<Entry>
    {
        public Entry GetByID(int id);

        public IEnumerable<Entry> GetByDictionaryAndWord(int dictionaryIndex, String wordValue);

        public bool ExistsByID(int id);

        /// <summary>
        /// Checks if theres already another Entry that contains Word of given ID
        /// </summary>
        /// <param name="wordID"></param>
        /// <returns></returns>
        public bool ExistsByWord(int wordID);
    }
}
