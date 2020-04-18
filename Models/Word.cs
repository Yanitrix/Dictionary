using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Models
{
    public class Word : IComparable<Word>
    {
        public int ID { get; set; }

        [Required]
        public Language SourceLanguage { get; set; }
        [ForeignKey("SourceLanguageID")]
        public int SourceLanguageID { get; set; }

        [Required]
        public String Value { get; set; }

        public ICollection<Meaning> Meanings { get; set; }

        [Required]
        public WordProperties Properties { get; set; }

        public int CompareTo([AllowNull] Word other)
        {
            return this.Value.CompareTo(other.Value);
        }
    }
}
