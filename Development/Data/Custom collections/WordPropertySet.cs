using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Data.Models
{
    public class WordPropertySet : HashSet<WordProperty>
    {
        public WordPropertySet():base(new WordPropertyEqualityComparer())
        {
        }
    }
}
