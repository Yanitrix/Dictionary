﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Api.Dto
{
    public class ExpressionDto
    {
        public int ID { get; set; }
        [Required]
        public String Text { get; set; }
        [Required]
        public String Translation { get; set; }
    }
}
