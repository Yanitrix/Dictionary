using System;
using System.Collections.Generic;

namespace Domain.Dto
{
    public class GetMeaning
    {
        public int ID { get; set; }

        public int EntryID { get; set; }

        public String Value { get; set; }

        public String Notes { get; set; }

        public ICollection<ExampleDto> Examples { get; set; } = new List<ExampleDto>();
    }
}
