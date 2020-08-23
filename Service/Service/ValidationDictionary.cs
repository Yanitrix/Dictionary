using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Service
{
    public class ValidationDictionary : IValidationDictionary
    {
        private readonly ModelStateDictionary modelState;

        public ValidationDictionary(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public void AddError(string key, string errorMsg)
        {
            modelState.AddModelError(key, errorMsg);
        }
    }
}