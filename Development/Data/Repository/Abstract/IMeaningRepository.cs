﻿using Data.Models;
using System;
using System.Collections.Generic;

namespace Data.Repository
{
    /// <summary>
    /// Every query includes Examples.
    /// </summary>
    public interface IMeaningRepository : IRepository<Meaning>
    {
        public Meaning GetByID(int id);

        public Meaning GetByIDWithEntry(int id);

        /// <summary>
        /// Including Entry
        /// </summary>
        /// <param name="value"></param>
        public IEnumerable<Meaning> GetByValueSubstring(String value);

        /// <summary>
        /// Including Entry
        /// </summary>
        /// <param name="notes"></param>
        public IEnumerable<Meaning> GetByNotesSubstring(String notes);

        /// <summary>
        /// Retrieves a collection of Meanings with similar value and given Dictionary.Index.
        /// Includes Entry
        /// </summary>
        /// <param name="dictionaryIndex"></param>
        /// <param name="valueSubstring"></param>
        public IEnumerable<Meaning> GetByDictionaryAndValueSubstring(int dictionaryIndex, String valueSubstring);
        
        public bool ExistsByID(int id);
    }
}
