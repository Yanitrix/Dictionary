using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Example : Expression
    {
        public int MeaningID { get; set; }
        public Meaning Meaning { get; set; }
    }
}
