using System;
using System.Collections.Generic;

namespace Domain.Dto
{
    public class UpdateMeaning
    {
        public int ID { get; set; }

        public String Value { get; set; }

        public String Notes { get; set; }

        public ICollection<ExampleDto> Examples { get; set; } = new List<ExampleDto>();
    }
}
