using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Dto
{
    public class SpeechPartDto
    {
        public int Index { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public String LanguageName { get; set; }

        [Required]
        public ICollection<SpeechPartPropertyDto> Properties { get; set; }
    }
}