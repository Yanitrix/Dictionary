using System;
using System.Collections.Generic;

namespace Data.Models
{

    /// <summary>
    /// <b>Please note: Equality comparer provided for this class is case insensitive.</b>
    /// </summary>
    public class StringSet : HashSet<String>
    {
        public StringSet(params String[] content) : base(content, new StringEqualityComparer())
        {
        }

        public override int GetHashCode()
        {
            if (Count == 0) return 0;

            int hash = 17;

            foreach(var e in this)
            {
                hash ^= e.ToUpper().GetHashCode();
            }

            return hash;
        }

    }
}
