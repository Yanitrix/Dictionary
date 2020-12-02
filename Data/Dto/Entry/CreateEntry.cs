using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Dto
{
    public class CreateEntry
    {
        [Required]
        public int DictionaryIndex { get; set; }

        [Required]
        public int WordID { get; set; }

    }
}
