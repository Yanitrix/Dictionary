using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dto
{
    public class CreateOrUpdateFreeExpression
    {
        [Required]
        public int DictionaryIndex { get; set; }

        [Required]
        public String Text { get; set; }

        [Required]
        public String Translation { get; set; }

    }
}
