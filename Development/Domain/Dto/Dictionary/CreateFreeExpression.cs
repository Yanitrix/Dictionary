using System;

namespace Domain.Dto
{
    public class CreateFreeExpression
    {
        public int DictionaryIndex { get; set; }

        public String Text { get; set; }

        public String Translation { get; set; }
    }
}
