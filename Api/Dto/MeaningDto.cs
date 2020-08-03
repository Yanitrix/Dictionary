using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Api.Dto
{
    public class MeaningDto
    {
        public int ID { get; set; }

        [Required]
        public int EntryID { get; set; }

        public String Value { get; set; }

        public ISet<ExpressionDto> Examples { get; set; }

        public String Notes { get; set; }

    }
}
