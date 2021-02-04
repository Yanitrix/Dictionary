using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Dto
{

    public class CreateMeaning
    {
        [Required]
        public int EntryID { get; set; }

        public String Value { get; set; }

        public String Notes { get; set; }

        public ICollection<ExampleDto> Examples { get; set; } = new List<ExampleDto>();
    }


}
