using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dictionary_MVC.Models
{
    public class SpeechPart
    {
        public int Index { get; set; }

        public String Name { get; set; }

        public Language Language { get; set; }
        [ForeignKey("Language")]
        public String LanguageName { get; set; }

        public ICollection<SpeechPartProperty> Properties { get; set; }
    }
}