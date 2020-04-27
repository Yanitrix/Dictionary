using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary_MVC.Models.annotations
{
    public class MeaningValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var meaning = (Meaning)validationContext.ObjectInstance;

            if (String.IsNullOrEmpty(meaning.Value) && String.IsNullOrEmpty(meaning.Example))
                return new ValidationResult("Either example or meaning must be present");
            
            return ValidationResult.Success;
        }
    }
}
