using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Dto
{
    public class CreateOrUpdateEntry
    {
        [Required]
        public int DictionaryIndex { get; set; }

        [Required]
        public int WordID { get; set; }

    }
}
