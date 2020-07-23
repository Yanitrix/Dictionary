using Dictionary_MVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Metadata.Annotations
{
    [Obsolete] //TODO remove
    public class MeaningValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var meaning = (Meaning)validationContext.ObjectInstance;

            if (String.IsNullOrEmpty(meaning.Value) && !meaning.Examples.Any())
                return new ValidationResult("Either examples or meaning must be present");
            
            return ValidationResult.Success;
        }
    }
}
