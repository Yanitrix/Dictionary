using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class FreeExpression : Expression
    {
        public Dictionary Dictionary { get; set; }
        public int DictionaryIndex { get; set; }
    }
}
