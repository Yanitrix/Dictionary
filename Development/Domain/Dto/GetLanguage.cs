using System;
using System.Collections.Generic;

namespace Domain.Dto
{
    public class GetLanguage
    {
        public String Name { get; set; }
        public ICollection<GetWord> Words { get; set; } = new List<GetWord>();
    }
}
