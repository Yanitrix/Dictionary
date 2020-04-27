using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Models
{
    public class Dictionary
    {
        [ForeignKey("LanguageIn")]
        public String LanguageInName { get; set; }
        [Required]
        public Language LanguageIn { get; set; }

        [ForeignKey("LanguageOut")]
        public String LanguageOutName { get; set; }
        [Required]
        public Language LanguageOut { get; set; }

        public ISet<Entry> Entries { get; set; }

    }
}
