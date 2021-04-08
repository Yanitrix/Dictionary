﻿using Domain.Models;
using System;
using System.Collections.Generic;

namespace Domain.Repository
{
    public interface IEntryRepository : IRepository<Entry, int>
    {
        /// <summary>
        /// Including Word and its Properties, Meanings and their Examples, Dictionary.
        /// <i>wordValue</i> is case insensitive
        /// </summary>
        /// <param name="dictionaryIndex"></param>
        /// <param name="wordValue"></param>
        /// <returns>Matching collection</returns>
        public IEnumerable<Entry> GetByDictionaryAndWord(int dictionaryIndex, String wordValue, bool caseSensitive = true);

        /// <summary>
        /// Including Word and its Properties, Meanings and their Examples, Dictionary
        /// <i>wordValue</i> is case insensitive
        /// </summary>
        /// <param name="wordValue"></param>
        /// <returns></returns>
        public IEnumerable<Entry> GetByWord(String wordValue, bool caseSensitive = true);

        /// <summary>
        /// Returns all Entries owned by certain Dictionary
        /// Including Word and its Properties, Meanings and their Examples, Dictionary
        /// </summary>
        /// <param name="dictionaryIndex"></param>
        /// <returns></returns>
        public IEnumerable<Entry> GetByDictionary(int dictionaryIndex);

        public bool HasMeanings(int id);
    }
}
