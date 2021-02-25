using System.Collections.Generic;

namespace Domain.Dto
{
    public class BidirectionalTranslationResponse
    {
        //so a list of Entries for both Dictionaries
        public IEnumerable<GetEntry> BaseResultEntries { get; set; }

        public IEnumerable<GetEntry> OppositeResultEntries { get; set; }

        //now, separate lists for FreeExpressions
        public IEnumerable<GetFreeExpression> BaseResultExpressions { get; set; }

        public IEnumerable<GetFreeExpression> OppositeResultExpressions { get; set; }
    }
}
