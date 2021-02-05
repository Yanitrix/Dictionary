using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class ValidationResultImpl : ValidationResult
    {
        private readonly List<ValidationError> errors = new();

        public ValidationResultImpl(object entity) : base(entity) { }

        public override bool IsValid => errors.Count == 0;

        public override void AddError(String key, String errorMsg)
        {
            errors.Add(new(key, errorMsg));
        }

        public override IEnumerator<ValidationError> GetEnumerator()
        {
            return errors.GetEnumerator();
        }
    }
}