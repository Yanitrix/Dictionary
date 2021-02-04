using Data.Models;
using System.Text.Json;
using Xunit;

namespace Data.Tests.Json
{
    public class StringSetConverter
    {

        [Fact]
        public void StringSetConverter_ShouldConvertProperly()
        {
            StringSet set = new("one", "two", "three");

            var str = JsonSerializer.Serialize(set, new JsonSerializerOptions { WriteIndented = true, Converters = { new Data.StringSetConverter() } });
            var obj = JsonSerializer.Deserialize<StringSet>(str, new JsonSerializerOptions { Converters = { new Data.StringSetConverter() } });
            Assert.NotNull(obj);

        }

    }
}
