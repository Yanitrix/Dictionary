using System;
using System.Collections.Generic;

namespace Data.Dto
{
    public class UpdateMeaning
    {
        public String Value { get; set; }

        public String Notes { get; set; }

        public ICollection<ExampleDto> Examples { get; set; } = new List<ExampleDto>();
    }
}
