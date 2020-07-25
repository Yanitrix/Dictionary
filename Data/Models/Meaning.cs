using Dictionary_MVC.Metadata.Annotations;
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

        [Required]
        public Entry Entry { get; set; }
        [Required]
        public int EntryID { get; set; }

        public String Value { get; set; } = String.Empty;

        public ISet<Expression> Examples { get; set; } = new HashSet<Expression>();

        public String Notes { get; set; }

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
