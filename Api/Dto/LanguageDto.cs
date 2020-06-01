using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Api.Dto
{
    public class LanguageDto
    {
        [Required]
        [RegularExpression("^[a-zA-Z]+$")]
        public String Name { get; set; }

        [Required]
        public ICollection<WordDto> Words { get; set; }

        [Required]
        public ICollection<SpeechPartDto> SpeechParts { get; set; }
    }

}
