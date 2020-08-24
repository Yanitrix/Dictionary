using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Dto
{
    public class LanguageDto
    {
        [Required]
        public String Name { get; set; }

    }

}
