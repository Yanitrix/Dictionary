using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Data.Models
{

    public class WordPropertyEqualityComparer : IEqualityComparer<WordProperty>
    {
        public bool Equals([AllowNull] WordProperty x, [AllowNull] WordProperty y)
        {
            if (x == y) return true;
            if (x == null && y == null) return true;
            if ((x == null) != (y == null)) return false;

            if (!String.Equals(x.Name.Trim(), y.Name.Trim(), StringComparison.OrdinalIgnoreCase)) return false;
            return x.Values.SetEquals(y.Values);
        }

        public int GetHashCode([DisallowNull] WordProperty obj)
        {
            if (obj == null) return 0;
            var hash = new HashCode();
            hash.Add(obj.Name.Trim().ToUpper());
            hash.Add(obj.Values?.GetHashCode());
            return hash.ToHashCode();
        }
    }
}
