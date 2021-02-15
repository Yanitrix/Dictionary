using Domain.Models;
using System;
using Xunit;

namespace Domain.Tests.Collections
{
    public class StringEqualityComparerTests
    {
        StringEqualityComparer comparer = new StringEqualityComparer();

        [Theory]
        [InlineData("string", "STRing")]
        [InlineData("nam strzelać nie kazano", "naM stRzelAĆ nIE KazAno")]
        [InlineData("lorem ipsum dolor SIT ameT", "Lorem IPSum dolor SIT AMEt")]
        [InlineData("word", "Word")]
        //edge case
        [InlineData(null, null)]
        public void SameString_IgnoresCase_EqualsReturnsTrue_HashCodeSame(String x, String y)
        {
            var xhash = comparer.GetHashCode(x);
            var yhash = comparer.GetHashCode(y);
            var equal = comparer.Equals(x, y);

            Assert.Equal(xhash, yhash);
            Assert.True(equal);
        }

        //edge cases
        [Theory]
        [InlineData(null, "hstus")]
        [InlineData("notnull", null)]
        [InlineData("lorem IPSum", "not lorem Ipsum")]
        [InlineData("LOREM IPSUM", "LOREM oPSUM")]
        public void StringDifferent_EqualsReturnsFalse_AndHashCodeShouldActuallyReturnFalseButWhoKnowsWhatItWillReturn(String x, String y)
        {
            var xhash = comparer.GetHashCode(x);
            var yhash = comparer.GetHashCode(y);
            var equal = comparer.Equals(x, y);

            Assert.NotEqual(xhash, yhash);
            Assert.False(equal);
        }
    }

}
