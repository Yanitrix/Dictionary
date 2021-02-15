using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Domain.Tests
{
    public class HashCodeTests
    {
        [Fact]
        public void AreSame()
        {
            var value1 = 1;
            var value2 = "hstus";
            var value3 = 32.87m;

            var hash1 = new HashCode();
            hash1.Add(value2);
            hash1.Add(value1);
            hash1.Add(value3);

            var hash2 = new HashCode();
            hash2.Add(value3);
            hash2.Add(value2);
            hash2.Add(value1);

            Assert.Equal(hash1.ToHashCode(), hash2.ToHashCode());
        }
    }
}
