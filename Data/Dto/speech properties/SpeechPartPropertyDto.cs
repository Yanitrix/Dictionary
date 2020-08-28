﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using System.Text;

namespace Data.Dto
{
    public class SpeechPartPropertyDto
    {
        [Required]
        public String Name { get; set; }

        [Required]
        public ISet<String> PossibleValues { get; set; } = new HashSet<String>();

    }
}
