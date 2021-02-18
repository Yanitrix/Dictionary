using System;
using System.Collections.Generic;

namespace Domain.Dto
{
    public class CreateWord
    {
        public String Value { get; set; }
 
        public String SourceLanguageName { get; set; }

        public ISet<WordPropertyDto> Properties { get; set; } = new HashSet<WordPropertyDto>();
    }
}
