using System;
using System.Collections.Generic;

namespace Domain.Dto
{
    public class GetWord
    {
        public int ID { get; set; }
        
        public String SourceLanguageName { get; set; }
        
        public String Value { get; set; }
        
        public ISet<WordPropertyDto> Properties { get; set; } = new HashSet<WordPropertyDto>();
    }
}