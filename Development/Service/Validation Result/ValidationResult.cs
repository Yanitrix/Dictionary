using System;
using System.Collections;
using System.Collections.Generic;

namespace Service
{
    public abstract class ValidationResult : IEnumerable<ValidationError>
    {
        protected ValidationResult(object entity)
        {
            Entity = entity;
        }

        public abstract void AddError(String errorName, String errorDescription);

        public object Entity { get; }

        public abstract bool IsValid { get; }

        public bool IsInvalid { get => !IsValid; }

        public static ValidationResult New(object entity) => new ValidationResultImpl(entity);

        public abstract IEnumerator<ValidationError> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ValidationError
    {
        public ValidationError(String name, String description)
        {
            Name = name;
            Description = description;
        }

        public String Name { get; }

        public String Description { get; }
    }
}