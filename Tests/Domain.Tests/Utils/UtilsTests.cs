using Domain.Models;
using Domain.Util;
using System.Linq;
using Xunit;

namespace Domain.Tests
{
    public class UtilsTests
    {
        [Fact]
        public void RemoveRedundantSpacesFromWordProperties_ShouldRemoveThem()
        {
            WordProperty[] wps =
            {
                new WordProperty
                {
                    Name = "  name  ",
                    Values = new StringSet{"  value1", " some  other    value 2"}
                },

                new WordProperty
                {
                    Name = " another  name",
                    Values = new StringSet{"    value3", "value  4"}
                }
            };

            Utils.RemoveRedundantWhitespaces(wps);

            Assert.Equal("name", wps[0].Name);
            Assert.Equal("value1", wps[0].Values.First());
            Assert.Equal("some other value 2", wps[0].Values.Last());

            Assert.Equal("another name", wps[1].Name);
            Assert.Equal("value3", wps[1].Values.First());
            Assert.Equal("value 4", wps[1].Values.Last());

        }
    }
}
