using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    [Obsolete]
    public enum ValidationResult : byte
    {
        ReadyToAdd,
        ReadyToUpdate,
        Failed
    }
}
