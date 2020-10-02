using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Data.Models
{
    public class WordProperty
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public HashSet<String> Values { get; set; } = new HashSet<String>();

        public override string ToString()
        {
            return $"Name: {Name} \t Value: {Values.Aggregate("", (left, right) => left + right)}\n";
        }
    }
}
