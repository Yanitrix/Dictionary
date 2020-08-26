using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace Api.Service
{
    public class ValidationDictionary : Dictionary<String,String>, IValidationDictionary
    {
        private readonly ModelStateDictionary modelState;

        public ValidationDictionary(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public void AddError(string key, string errorMsg)
        {
            modelState.AddModelError(key, errorMsg);
            Add(key, errorMsg);
        }
    }
}