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

        public int DictionaryIndex { get; set; }

        [Required]
        public Word Word { get; set; }

        [Required]
        public int WordID { get; set; }

        public ISet<Meaning> Meanings { get; set; } = new HashSet<Meaning>();

        public ISet<Expression> Expressions { get; set; } = new HashSet<Expression>();

        public override string ToString()
        {
            String mean = "", exp = "";
            foreach (var i in Meanings) mean += i.ToString();
            foreach (var i in Expressions) exp += i.ToString();

            return $"Entry:\n" +
                $"Word: \n\t{Word}\n" +
                $"Meanings: \n\t{mean}\n" +
                $"Expressions: {exp}\n";
        }
    }
}
