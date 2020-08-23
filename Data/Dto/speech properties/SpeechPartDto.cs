using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Api.Dto
{
    public class SpeechPartDto
    {
        [Required]
        public String Name { get; set; }

        [Required]
        public String LanguageName { get; set; }

        [Required]
        public IEnumerable<SpeechPartPropertyDto> Properties { get; set; } = Enumerable.Empty<SpeechPartPropertyDto>();
    }
}