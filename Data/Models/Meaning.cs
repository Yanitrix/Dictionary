using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Meaning
    {
        public int ID { get; set; }

        [Required]
        public Entry Entry { get; set; }
        [Required]
        public int EntryID { get; set; }

        //cannot be empty, if user wants to use only examples then they're encouraged to use dictionary-level Expression instead
        [Required]
        public String Value { get; set; } = String.Empty;

        public ICollection<Expression> Examples { get; set; } = new List<Expression>();

        public String Notes { get; set; } = String.Empty;

        public override string ToString()
        {
            var examples = "";
            foreach (var item in Examples)
            {
                examples += item.ToString();
            }

            return
                $"Value: {Value}\n" +
                $"Examples:\n { examples}\n"+
                $"Notes: {Notes}\n";
        }
    }
}
