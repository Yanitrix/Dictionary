using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Example
    {
        public int ID { get; set; }

        [Required]
        public String Text { get; set; }
        [Required]
        public String Translation { get; set; }

        public int MeaningID { get; set; }
        public Meaning Meaning { get; set; }
    }
}
