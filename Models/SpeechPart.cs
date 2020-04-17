using System;
using System.Collections.Generic;

namespace Dictionary_MVC.Models
{
    public class SpeechPart
    {
        public AttributePattern AttributePattern { get; set; }

        public String Name { get; set; }

        public ICollection<Property> Properties { get; set; }




        public class Property
        {
            public SpeechPart SpeechPart { get; set; }

            public String Name { get; set; }

            public String DefaultValue { get; set; }


        }
    }
}