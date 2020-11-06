using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class FreeExpression
    {
        public int ID { get; set; }

        [Required]
        public String Text { get; set; }
        [Required]
        public String Translation { get; set; }

        public Dictionary Dictionary { get; set; }
        public int DictionaryIndex { get; set; }
    }
}
