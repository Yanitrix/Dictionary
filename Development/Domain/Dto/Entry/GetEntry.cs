using System.Collections.Generic;

namespace Domain.Dto
{
    public class GetEntry
    {
        public int ID { get; set; }

        public GetDictionary Dictionary { get; set; }

        public GetWord Word { get; set; }

        public ICollection<GetMeaning> Meanings { get; set; } = new List<GetMeaning>();
    }
}
