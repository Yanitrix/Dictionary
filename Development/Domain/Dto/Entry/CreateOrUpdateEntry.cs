using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dto
{
    public class CreateOrUpdateEntry
    {
        [Required]
        public int DictionaryIndex { get; set; }

        [Required]
        public int WordID { get; set; }

    }
}
