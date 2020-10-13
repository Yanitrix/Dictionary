﻿using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Data.Util
{
    public static class Utils
    {
        public static void RemoveRedundantWhitespaces(IEnumerable<WordProperty> wp)
        {
            Regex regex = new Regex("[ ]{2,}", RegexOptions.None);

            foreach(var i in wp)
            {
                i.Name = regex.Replace(i.Name, " ").Trim();
                var @new = new HashSet<String>();
                foreach(var j in i.Values)
                {
                    @new.Add(regex.Replace(j, " ").Trim());
                }
                i.Values = @new;
            }
        }

    }
}
