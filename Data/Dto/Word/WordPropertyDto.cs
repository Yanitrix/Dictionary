using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Dto
{
    //todo maybe simplify dto so that in only contains Map<Name, Value>?
    public class WordPropertyDto
    {
        [Required]
        public String Name { get; set; }

        [Required]
        public String Value { get; set; }
    }
}
