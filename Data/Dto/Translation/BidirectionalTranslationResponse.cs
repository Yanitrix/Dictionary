using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Dto
{
    public class BidirectionalTranslationResponse
    {
        //so a list of Entries for both Dictionaries

        public IEnumerable<GetEntry> ResultEntries { get; set; }

        public IEnumerable<GetEntry> OppositeResultEntries { get; set; }

        //now, separate lists for FreeExpressions

        public IEnumerable<GetFreeExpression> ResultExpressions { get; set; }

        public IEnumerable<GetFreeExpression> OppositeResultExpressions { get; set; }
    }
}
