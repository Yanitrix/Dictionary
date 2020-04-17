using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Models
{
    public class Word : IComparable<Word>
    {
        public Language SourceLanguage { get; set; }

        [Required]
        public String Value { get; set; }

        public WordProperties Properties { get; set; }

        public int CompareTo([AllowNull] Word other)
        {
            return this.Value.CompareTo(other.Value);
        }
    }
}
