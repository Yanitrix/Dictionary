using Data.Models;
using System;
using System.Collections.Generic;

namespace Service.Repository
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
        /// <returns></returns>
        public IEnumerable<Meaning> GetByValueSubstring(String value);

        /// <summary>
        /// Including Entry
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
        public IEnumerable<Meaning> GetByNotesSubstring(String notes);

        public bool ExistsByID(int id);
    }
}
