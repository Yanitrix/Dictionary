using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Domain.Models
{
    public class WordPropertySet : HashSet<WordProperty>
    {
        public WordPropertySet():base(new WordPropertyEqualityComparer())
        {
        }
    }
}
