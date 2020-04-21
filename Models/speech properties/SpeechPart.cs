﻿using System;
using System.Collections.Generic;

namespace Dictionary_MVC.Models
{
    public class SpeechPart
    {
        public int ID { get; set; }

        public String Name { get; set; }

        public ICollection<SpeechPartProperty> Properties { get; set; }
    }
}