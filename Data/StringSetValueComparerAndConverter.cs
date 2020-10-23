using Data.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Data
{
    public static class StringSetValueComparerAndConverter
    {
        public static ValueComparer<StringSet> Comparer { get; } = new
            ValueComparer<StringSet>(
                (o1, o2) => o1.OrderBy(x => x).SequenceEqual(o2.OrderBy(x => x)),
                instance => instance.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                instance => new StringSet(instance.ToHashSet<String>().ToArray())
            );

        public static ValueConverter<StringSet, String> Converter { get; } = new
            ValueConverter<StringSet, String>(
                source => JsonConvert.SerializeObject(source, Formatting.Indented, new StringSetConverter()),
                dest => JsonConvert.DeserializeObject<StringSet>(dest, new StringSetConverter())
            );

    }

    //TODO that should be tested
    public class StringSetConverter : JsonConverter<StringSet>
    {
        public override StringSet ReadJson(JsonReader reader, Type objectType, [AllowNull] StringSet existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var val = (String)reader.Value;
            return new StringSet(JsonConvert.DeserializeObject<HashSet<String>>(val).ToArray());
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] StringSet value, JsonSerializer serializer)
        {
            JsonConvert.SerializeObject(value.ToHashSet());
        }
    }
}
