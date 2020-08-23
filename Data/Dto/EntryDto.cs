using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Dto
{
    public class EntryDto
    {
        public int ID { get; set; }

        [Required]
        public int DictionaryIndex { get; set; }

        [Required]
        public int WordID { get; set; }

    }
}
