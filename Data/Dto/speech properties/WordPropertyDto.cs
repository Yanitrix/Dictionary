using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Dto
{
    public class WordPropertyDto
    {
        public int ID { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public String Value { get; set; }
    }
}
