﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Data
{
    public static class StringSetValueComparerAndConverter
    {
        public static ValueComparer<HashSet<String>> Comparer { get; } = new
            ValueComparer<HashSet<String>>(
                (o1, o2) => o1.OrderBy(x => x).SequenceEqual(o2.OrderBy(x => x)),
                instance => instance.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                instance => instance.ToHashSet<String>()
            );

        public static ValueConverter<HashSet<String>, String> Converter { get; } = new
            ValueConverter<HashSet<String>, String>(
                source => JsonConvert.SerializeObject(source),
                dest => JsonConvert.DeserializeObject<HashSet<String>>(dest)
            );

    }
}
