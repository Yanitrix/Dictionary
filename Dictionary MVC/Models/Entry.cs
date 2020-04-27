using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Models
{
    public class Entry
    {
        public int ID { get; set; }

        [Required]
        public Dictionary Dictionary { get; set; }

        [Required]
        public Word Word { get; set; }

        public ISet<Meaning> Meanings { get; set; }
    }
}
