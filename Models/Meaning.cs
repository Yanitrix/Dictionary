using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Models
{
    public class Meaning
    {
        public Entry Entry { get; set; }

        public String Value { get; set; }

        public String Example { get; set; }

        public String Notes { get; set; }
    }
}
