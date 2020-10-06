using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class ValidationDictionary : Dictionary<String, String>, IValidationDictionary
    {
        private readonly ModelStateDictionary modelState;

        public ValidationDictionary() { }

        public ValidationDictionary(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public void AddError(string key, string errorMsg)
        {
            modelState?.AddModelError(key, errorMsg);
            Add(key, errorMsg);
        }

        public bool IsValid { get => !this.Any(); }
    }
}