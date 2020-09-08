using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Language
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [RegularExpression("^[a-zA-Z]+$")]
        public String Name { get; set; }

        public ISet<Word> Words { get; set; } = new HashSet<Word>();

        public ICollection<SpeechPart> SpeechParts { get; set; } = new List<SpeechPart>();

    }
}
