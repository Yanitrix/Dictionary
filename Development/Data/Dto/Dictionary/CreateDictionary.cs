﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Dto
{
    public class CreateDictionary
    {

        [Required]
        public String LanguageIn { get; set; }

        [Required]
        public String LanguageOut { get; set; }

    }
}
