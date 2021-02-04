using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Dto
{
    public class CreateLanguage
    {
        [Required]
        public String Name { get; set; }
    }
}
