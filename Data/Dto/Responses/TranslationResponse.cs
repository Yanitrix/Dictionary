using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Dto
{
    public class TranslationResponse
    {
        //so a list of Entries for both Dictionaries

        public IEnumerable<Entry> DictionaryResultEntries { get; set; }

        public IEnumerable<Entry> OppositeResultEntries { get; set; }

        //now, separate lists for FreeExpressions

        public IEnumerable<Expression> ResultExpressions { get; set; }

        public IEnumerable<Expression> OppositeResultExpressions { get; set; }
    }
}
