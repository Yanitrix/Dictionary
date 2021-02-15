using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Models
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
        public WordPropertySet Properties { get; set; } = new WordPropertySet();

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
                $"Properties: \n\t{prop}\n";
        }


    }
}
