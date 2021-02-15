﻿using Domain.Models;
using System.Text.Json;
using Xunit;

namespace Persistence.Tests.Json
{
    public class StringSetConverter
    {
        [Fact]
        public void StringSetConverter_ShouldConvertProperly()
        {
            StringSet set = new("one", "two", "three");

            var str = JsonSerializer.Serialize(set, new JsonSerializerOptions { WriteIndented = true, Converters = { new Persistance.StringSetConverter() } });
            var obj = JsonSerializer.Deserialize<StringSet>(str, new JsonSerializerOptions { Converters = { new Persistance.StringSetConverter() } });
            Assert.NotNull(obj);

        }
    }
}
