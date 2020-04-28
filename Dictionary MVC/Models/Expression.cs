using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Models
{
    public class Expression
    {
        public int ID { get; set; }
        public String Text { get; set; }
        public String Translation { get; set; }

        public override string ToString()
        {
            return $"Text: {Text} \t Translation: {Translation}\n";
        }
    }
}
