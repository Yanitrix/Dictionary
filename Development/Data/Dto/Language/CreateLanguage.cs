using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dto
{
    public class CreateLanguage
    {
        [Required]
        public String Name { get; set; }
    }
}
