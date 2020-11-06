using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Dictionary
    {
        public int Index { get; set; }

        [ForeignKey("LanguageIn")]
        public String LanguageInName { get; set; }
        [Required]
        public Language LanguageIn { get; set; }

        [ForeignKey("LanguageOut")]
        public String LanguageOutName { get; set; }
        [Required]
        public Language LanguageOut { get; set; }

        public ISet<Entry> Entries { get; set; } = new HashSet<Entry>();

        public ICollection<FreeExpression> FreeExpressions { get; set; } = new List<FreeExpression>();

        public override string ToString()
        {
            var entr = "";
            foreach (var i in Entries) entr += i.ToString();

            return "Dictionary \n" +
                $"Language in: {LanguageInName}\n" +
                $"Language out: {LanguageOutName}\n" +
                $"Entries: \n\t{entr}\n";
        }
    }
}
