using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Data.Tests.Collections
{
    public class StringSetTests
    {


        [Fact]
        public void GetHashCodeIsTheSame()
        {
            var set1 = new StringSet("value1", "VALue2", "valUE3");
            var set2 = new StringSet("VALue1", "value3", "VALUE2");

            var hash1 = set1.GetHashCode();
            var hash2 = set2.GetHashCode();

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void SetEqualsReturnsTrue()
        {
            var set1 = new StringSet("value1", "VALue2", "valUE3");
            var set2 = new StringSet("VALue1", "value3", "VALUE2");

            var equal = set1.SetEquals(set2);
            Assert.True(equal);
        }
    }
}
