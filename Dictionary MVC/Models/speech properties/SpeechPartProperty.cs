using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dictionary_MVC.Models
{
    public class SpeechPartProperty
    {
        public int ID { get; set; }

        public SpeechPart SpeechPart { get; set; }

        public String Name { get; set; }

        [NotMapped]
        public String DefaultValue
        {
            get { return PossibleValues[0]; }
            //set { PossibleValues[0] = value; }
        }

        /// <summary>
        /// First value of the list is the default value for a Property.
        /// </summary>
        public IList<String> PossibleValues { get; set; } = new List<String>();

    }
}
