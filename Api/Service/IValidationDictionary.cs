using System;

namespace Api.Service
{
    public interface IValidationDictionary
    {
        void AddError(String key, String errorMsg);
    }
}