using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Dto
{
    public class UpdateWord
    {
        [Required]
        public String Value { get; set; }

        public ISet<WordPropertyDto> Properties { get; set; } = new HashSet<WordPropertyDto>();
    }
}
