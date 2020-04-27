using Dictionary_MVC.Models.annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Models
{
    public class Meaning
    {
        public int ID { get; set; }

        //public Word Word { get; set; }  I don't actually know if it's needed, I think that not but that may change in the future.

        [Required]
        public Entry Entry { get; set; }

        [MeaningValidation]
        public String Value { get; set; } = String.Empty;

        [MeaningValidation]
        public String Example { get; set; } = String.Empty;

        public String Notes { get; set; }
    }
}
