using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Dictionary_MVC.Models
{
    public class SpeechPart
    {
        public int Index { get; set; }

        public String Name { get; set; }

        public Language Language { get; set; }
        [ForeignKey("Language")]
        public String LanguageName { get; set; }

        public IEnumerable<SpeechPartProperty> Properties { get; set; } = Enumerable.Empty<SpeechPartProperty>();
    }
}