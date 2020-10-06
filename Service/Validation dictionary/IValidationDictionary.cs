﻿using System;
using System.Collections.Generic;

namespace Service
{
    public interface IValidationDictionary : IDictionary<String, String>
    {
        void AddError(String key, String errorMsg);

        bool IsValid { get; }
    }
}