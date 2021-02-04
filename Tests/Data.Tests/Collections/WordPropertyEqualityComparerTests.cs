using Data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Data.Tests.Collections
{
    public class WordPropertyEqualityComparerTests
    {
        WordPropertyEqualityComparer comparer = new WordPropertyEqualityComparer();


        [Theory]
        [ClassData(typeof(ThoseThatAreEqual))]
        public void NameOrOneOfValuesDiffersOnlyInCase_EqualReturnsTrue_HashCodeIsTheSame(WordProperty one, WordProperty another)
        {
            var xhash = comparer.GetHashCode(one);
            var yhash = comparer.GetHashCode(another);
            var equal = comparer.Equals(one, another);

            Assert.Equal(xhash, yhash);
            Assert.True(equal);

        }

        [Theory]
        [ClassData(typeof(ThoseThatAreNotEqual))]
        public void NameOrOneOfValuesDifferNotOnlyInCase_EqualReturnsFalse_HashCodeShouldBeDifferentButWhoKnowsIfItWill(WordProperty one, WordProperty another)
        {
            var xhash = comparer.GetHashCode(one);
            var yhash = comparer.GetHashCode(another);
            var equal = comparer.Equals(one, another);

            Assert.NotEqual(xhash, yhash);
            Assert.False(equal);
        }
    }

    public class ThoseThatAreEqual : IEnumerable<object[]>
    {
        private readonly List<object[]> data = new List<object[]>
        {
            new object[]
            {
                new WordProperty
                {
                    Name = "Name",
                    Values = new StringSet("value1", "value2", "value3")
                },

                //different order

                new WordProperty
                {
                    Name = "name",
                    Values = new StringSet("value2", "value3", "value1")
                }
            },

            new object[]
            {
                new WordProperty
                {
                    Name = "StrINg",
                    Values = new StringSet("Value2", "VaLuE1", "Val42")
                },

                new WordProperty
                {
                    Name = "stRINg",
                    Values = new StringSet("VALue2", "vALUe1", "vAl42")
                }
            },

            new object[]
            {
                new WordProperty
                {
                    Name = "haShTabLe",
                    Values = new StringSet("Name01", "na mE11", "naME10")
                },

                new WordProperty
                {
                    Name = "HASHtablE",
                    Values = new StringSet("NA ME11", "NamE01", "name10")
                }
            },

            //edge case
            new object[]{null, null}
        };


        public IEnumerator<object[]> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

    }


    public class ThoseThatAreNotEqual : IEnumerable<object[]>
    {
        private readonly List<object[]> data = new List<object[]>
        {
            new object[]
            {
                new WordProperty
                {
                    Name = "that a name",
                    Values = new StringSet("a value", "another value")
                },

                new WordProperty
                {
                    Name = "thats not a name",
                    Values = new StringSet(),
                },

            },

            new object[]
            {
                new WordProperty
                {
                    Name = "a name",
                    Values = new StringSet("Value1", "value2", "value3")
                },

                new WordProperty
                {
                    Name = "a name",
                    Values = new StringSet("Value1", "Value3")
                }
            },

            //edge cases
            new object[]{null, new WordProperty { Name = ""} },
            new object[]{ new WordProperty {Name = "" }, null}
        };

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();
    }
}
