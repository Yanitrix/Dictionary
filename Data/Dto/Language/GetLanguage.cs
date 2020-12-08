using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Dto
{
    public class GetLanguage
    {
        public String Name { get; set; }
        public ICollection<GetWord> Words { get; set; }
    }
}
