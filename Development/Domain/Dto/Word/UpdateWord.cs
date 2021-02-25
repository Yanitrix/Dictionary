using System;
using System.Collections.Generic;

namespace Domain.Dto
{
    public class UpdateWord
    {
        public int ID { get; set; }

        public String Value { get; set; }

        public ISet<WordPropertyDto> Properties { get; set; } = new HashSet<WordPropertyDto>();
    }
}
