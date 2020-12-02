using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Dto
{
    public class CreateWord
    {
        [Required]
        public String Value { get; set; }

        [Required]
        public String SourceLanguageName { get; set; }

        public ISet<WordPropertyDto> Properties { get; set; } = new HashSet<WordPropertyDto>();
    }
}
