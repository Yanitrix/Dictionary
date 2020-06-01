using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Models
{
    public class Language
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [RegularExpression("^[a-zA-Z]+$")]
        public String Name { get; set; }

        public ICollection<Word> Words { get; set; } = new HashSet<Word>();

        public ISet<SpeechPart> SpeechParts { get; set; } = new HashSet<SpeechPart>();

    }
}
