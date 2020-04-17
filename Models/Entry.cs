using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Models
{
    public class Entry
    {
        public Dictionary Dictionary { get; set; }

        public Word Word { get; set; }

        public ISet<Meaning> Meanings { get; set; }
    }
}
