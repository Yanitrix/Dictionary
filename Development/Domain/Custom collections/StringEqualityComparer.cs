using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Domain.Models
{

    public class StringEqualityComparer : IEqualityComparer<String>
    {
        public bool Equals([AllowNull] string x, [AllowNull] string y) => String.Equals(x, y, StringComparison.OrdinalIgnoreCase);

        public int GetHashCode([DisallowNull] string obj) => obj != null ? obj.ToLower().GetHashCode() : -1;

    }
}
