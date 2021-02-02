using System.Collections.Generic;

namespace Data.Dto
{
    public class TranslationResponse
    {
        //so a list of Entries for both Dictionaries

        public IEnumerable<GetEntry> ResultEntries { get; set; }

        public IEnumerable<GetFreeExpression> ResultExpressions { get; set; }
    }
}
