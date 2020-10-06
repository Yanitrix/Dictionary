using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    public interface IMeaningRepository : IRepository<Meaning>
    {
        public Meaning GetByID(int id);

        public Meaning GetByIDWithEntry(int id);

        public IEnumerable<Meaning> GetByValueSubstring(String value);

        public IEnumerable<Meaning> GetByNotesSubstring(String notes);

        public bool ExistsByID(int id);
    }
}
