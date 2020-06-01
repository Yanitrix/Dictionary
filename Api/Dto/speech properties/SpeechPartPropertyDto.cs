using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Api.Dto
{
    public class SpeechPartPropertyDto
    {
        [Required]
        public String Name { get; set; }

        [Required]
        public IList<String> PossibleValues { get; set; } = new List<String>();

    }
}
