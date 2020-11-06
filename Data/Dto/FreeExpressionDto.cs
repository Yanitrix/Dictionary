using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Dto
{
    public class FreeExpressionDto
    {
        public int ID { get; set; }

        public int DictionaryIndex { get; set; }

        [Required]
        public String Text { get; set; }
        [Required]
        public String Translation { get; set; }
    }
}
