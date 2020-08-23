using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Models
{
    public class WordProperty
    {
        public int ID { get; set; }
        [Required]

        public Word Word { get; set; }

        [Required]
        public int WordID { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public String Value { get; set; }

        public override string ToString()
        {
            return $"Name: {Name} \t Value: {Value}\n";
        }
    }
}
