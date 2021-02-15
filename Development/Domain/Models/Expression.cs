using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Expression
    {
        public int ID { get; set; }

        [Required]
        public String Text { get; set; }
        [Required]
        public String Translation { get; set; }
    }
}
