using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dto
{
    public class ExampleDto
    {
        [Required]
        public String Text { get; set; }
        [Required]
        public String Translation { get; set; }
    }
}
