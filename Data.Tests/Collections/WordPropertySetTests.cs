using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Data.Tests.Collections
{
    public class WordPropertySetTests
    {
        [Fact]
        public void SetEqualsShouldReturnTrue()
        {
            var word1 = new Word
            {
                Value = "value", //lowercase
                Properties = new WordPropertySet
                {
                    new WordProperty
                    {
                        Name = "name1 ",
                        Values = new StringSet("value1", "value2")
                    },

                    new WordProperty
                    {
                        Name = "name2",
                        Values = new StringSet("value3", "value4")
                    },
                }
            };

            var word2 = new Word
            {
                Value = "Value", //uppercase
                Properties = new WordPropertySet
                {
                    new WordProperty
                    {
                        Name = "name1",
                        Values = new StringSet("value1", "value2")
                    },

                    new WordProperty
                    {
                        Name = "name2",
                        Values = new StringSet("value3", "value4")
                    },
                }
            };

            HashSet<WordProperty> props1 = new HashSet<WordProperty>(new WordPropertyEqualityComparer())
            {
                    new WordProperty
                    {
                        Name = " name1",
                        Values = new StringSet("value1", "value2")
                    },

                    new WordProperty
                    {
                        Name = "name2",
                        Values = new StringSet("value3", "value4")
                    },
            };


            HashSet<WordProperty> props2 = new HashSet<WordProperty>(new WordPropertyEqualityComparer())
            {
                    new WordProperty
                    {
                        Name = "name1",
                        Values = new StringSet("value1", "value2")
                    },

                    new WordProperty
                    {
                        Name = "name2",
                        Values = new StringSet("value3", "value4")
                    },
            };

            var areEqual = props1.SetEquals(props2);
            var wordEqual = word1.Properties.SetEquals(word2.Properties);

            Assert.True(areEqual);
            Assert.True(wordEqual);
        }

    }
}
