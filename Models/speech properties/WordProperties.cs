using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Models
{
    public class WordProperties
    {
        public Word Word { get; set; }

        public ICollection<Property> Properties { get; set; }




        public class Property
        {
            public WordProperties WordProperties { get; set; }

            public String Name { get; set; }

            public String Value { get; set; }
        }
    }
}
