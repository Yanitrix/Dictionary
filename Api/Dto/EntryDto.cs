using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Api.Dto
{
    public class EntryDto
    {
        public int ID { get; set; }

        [Required]
        public int DictionaryIndex { get; set; }

        [Required]
        public int WordID { get; set; }

        [Required]
        public ISet<MeaningDto> Meanings { get; set; }

        public ISet<ExpressionDto> Expressions { get; set; } = new HashSet<ExpressionDto>();
    }
}
