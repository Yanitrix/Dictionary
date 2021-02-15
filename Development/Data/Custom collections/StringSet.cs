using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{

    /// <summary>
    /// <b>Please note: Equality comparer provided for this class is case insensitive.</b>
    /// </summary>
    public class StringSet : HashSet<String>
    {
        public StringSet(params String[] content) : base(content, new StringEqualityComparer())
        {
        }

        public StringSet():base(new StringEqualityComparer()){}

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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            foreach (var i in this)
                sb.Append(i + ",");
            sb.Append(']');
            return sb.ToString();
        }
    }
}
