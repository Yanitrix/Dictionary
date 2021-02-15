using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dto
{
    public class GetWord
    {
        public int ID { get; set; }

        [Required]
        public String SourceLanguageName { get; set; }

        [Required]
        public String Value { get; set; }

        [Required]
        public ISet<WordPropertyDto> Properties { get; set; } = new HashSet<WordPropertyDto>();

    }
}