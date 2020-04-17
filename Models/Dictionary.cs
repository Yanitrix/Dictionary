using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Models
{
    public class Dictionary
    {
        public Language LanguageIn { get; set; }

        public Language LanguageOut { get; set; }

        public ISet<Entry> Entries { get; set; }
    }
}
