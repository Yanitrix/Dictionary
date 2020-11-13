using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Dto
{
    public class ExampleDto
    {
        public int ID { get; set; }

        [Required]
        public String Text { get; set; }
        [Required]
        public String Translation { get; set; }
    }
}
