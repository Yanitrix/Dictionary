﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Dto
{
    public class GetEntry
    {
        public int ID { get; set; }

        public int DictionaryIndex { get; set; }

        public GetWord Word { get; set; }

        public ICollection<GetMeaning> Meanings { get; set; } = new List<GetMeaning>();
    }
}
