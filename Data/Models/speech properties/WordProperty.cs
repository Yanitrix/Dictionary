using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Models
{
    public class WordProperty
    {
        public Word Word { get; set; }

        public int WordID { get; set; }

        public int ID { get; set; }

        public String Name { get; set; }

        public String Value { get; set; }

        public override string ToString()
        {
            return $"Name: {Name} \t Value: {Value}\n";
        }
    }
}
