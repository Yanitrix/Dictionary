using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Models
{
    public class Language
    {
        [Key]
        public String Name { get; set; }

        public ICollection<Word> Words { get; set; }

        public AttributePattern AttributePattern { get; set; }
    }
}
