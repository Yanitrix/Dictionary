using Dictionary_MVC.Metadata.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Models
{
    public class Meaning //TODO while adding on front check if any expression contains the word and suggest them to the user
    {
        public int ID { get; set; }

        //public Word Word { get; set; }  I don't actually know if it's needed, I think that not but that may change in the future.

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
