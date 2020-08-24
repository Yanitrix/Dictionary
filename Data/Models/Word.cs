using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Word : IComparable<Word>
    {
        public int ID { get; set; }

        [Required]
        public Language SourceLanguage { get; set; }

        [ForeignKey("SourceLanguage")]
        public String SourceLanguageName { get; set; }

        [Required]
        public String Value { get; set; }

        [Required]
        public String SpeechPartName { get; set; }

        [Required]
        public IEnumerable<WordProperty> Properties { get; set; } = Enumerable.Empty<WordProperty>();

        public int CompareTo([AllowNull] Word other)
        {
            return this.Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            var prop = "";
            foreach (var i in Properties) prop += i.ToString();
            return
                $"Source language: {SourceLanguageName}\n" +
                $"Value: {Value}\n" +
                $"Speech part: {SpeechPartName}\n" +
                $"Properties: \n\t{prop}\n";
        }


    }
}
