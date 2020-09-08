using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Data.Models
{
    public class SpeechPart
    {
        public int Index { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public Language Language { get; set; }
        [ForeignKey("Language")]
        public String LanguageName { get; set; }

        public ICollection<SpeechPartProperty> Properties { get; set; } = new List<SpeechPartProperty>();
    }
}