using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Models
{
    public class AttributePattern
    {
        [Key]
        public Language Language { get; set; }

        public ISet<SpeechPart> PartsOfSpeech { get; set; }
    }
}
