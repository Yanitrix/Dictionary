using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Example : Expression
    {
        public int MeaningID { get; set; }
        public Meaning Meaning { get; set; }
    }
}
