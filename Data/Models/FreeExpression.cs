using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class FreeExpression : Expression
    {
        public Dictionary Dictionary { get; set; }
        public int DictionaryIndex { get; set; }
    }
}
