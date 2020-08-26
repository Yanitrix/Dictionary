using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

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
            Add(key, errorMsg);
        }
    }
}