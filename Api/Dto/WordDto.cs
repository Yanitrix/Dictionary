using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Dto
{
    public class WordDto
    {
        public int ID { get; set; }

        [Required]
        public String SourceLanguageName { get; set; }

        [Required]
        public String Value { get; set; }

        [Required]
        public String SpeechPartName { get; set; }

        [Required]
        public ISet<WordPropertyDto> Properties { get; set; }
    }
}