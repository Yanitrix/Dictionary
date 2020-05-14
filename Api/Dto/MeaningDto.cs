using Dictionary_MVC.Metadata.Annotations;
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

        [MeaningValidation]
        public String Value { get; set; }

        [MeaningValidation]
        public ISet<ExpressionDto> Examples { get; set; }

        public String Notes { get; set; }

    }
}
