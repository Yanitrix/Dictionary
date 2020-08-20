using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Dictionary_MVC.Models
{
    public class SpeechPartProperty
    {
        public int ID { get; set; }

        [Required]
        public SpeechPart SpeechPart { get; set; }
        [Required]
        public int SpeechPartIndex { get; set; }

        [Required]
        public String Name { get; set; }

        [NotMapped]
        public String DefaultValue
        {
            get { return PossibleValues.ToArray()[0]; }
        }

        /// <summary>
        /// First value of the list is the default value for a Property.
        /// </summary>
        public ISet<String> PossibleValues { get; set; } = new HashSet<String>();

    }
}
