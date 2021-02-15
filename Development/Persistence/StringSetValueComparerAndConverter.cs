using Domain.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Persistence
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
                stringSet => JsonSerializer.Serialize(stringSet, new JsonSerializerOptions { WriteIndented = true, Converters = { new StringSetConverter() } }),
                jsonString => JsonSerializer.Deserialize<StringSet>(jsonString, new JsonSerializerOptions { Converters = { new StringSetConverter() } })
            );

    }

    public class StringSetConverter : JsonConverter<StringSet>
    {
        public override StringSet Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = new StringSet();
            reader.Read();
            while(reader.TokenType != JsonTokenType.EndArray)
            {
                result.Add(reader.GetString());
                reader.Read();
            }

            return result;

        }

        public override void Write(Utf8JsonWriter writer, StringSet value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var i in value)
                writer.WriteStringValue(i);
            writer.WriteEndArray();

        }
    }
}
