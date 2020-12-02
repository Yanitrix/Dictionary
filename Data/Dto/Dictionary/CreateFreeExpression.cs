using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Dto
{
    public class CreateFreeExpression
    {
        [Required]
        public int DictionaryIndex { get; set; }

        [Required]
        public String Text { get; set; }

        [Required]
        public String Translation { get; set; }

    }
}
