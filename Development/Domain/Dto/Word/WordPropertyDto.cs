using System;
using System.Collections.Generic;

namespace Domain.Dto
{
    //todo maybe simplify dto so that in only contains Map<Name, Value>?
    public class WordPropertyDto
    {
        public String Name { get; set; }

        public ICollection<String> Values { get; set; }
    }
}
