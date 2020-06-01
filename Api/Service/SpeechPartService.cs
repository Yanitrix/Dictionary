using Api.Validation;
using Dictionary_MVC.Data;
using Dictionary_MVC.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Service
{
    public class SpeechPartService : ServiceBase<SpeechPart>, ISpeechPartService
    {
        private readonly AbstractValidator<SpeechPart> validator;

        public SpeechPartService(DatabaseContext context) : base(context)
        {
        }

        public SpeechPartService(DatabaseContext context, SpeechPartValidator validator) : this(context)
        {
            this.validator = validator;
        }

        public SpeechPart GetByIndex(int index)
        {
            return repo.Where(x => x.Index == index).SingleOrDefault();
        }

        public SpeechPart GetByNameAndLanguage(string languageName, string name)
        {
            return repo.SingleOrDefault(x => String.Equals(x.LanguageName, languageName, StringComparison.OrdinalIgnoreCase) && x.Name == name);
        }

        public override bool IsValid(SpeechPart entity)
        {
            var result = validator.Validate(entity);
            if (!result.IsValid)
            {
                foreach (var err in result.Errors) ValidationDictionary.AddError(err.PropertyName, err.ErrorMessage);
                return false;
            }
            return result.IsValid;
        }

        public override bool IsReadyToAdd(SpeechPart entity)
        {
            var res = IsValid(entity);
            if (!res) return false;
            if (GetByNameAndLanguage(entity.LanguageName, entity.Name) != null)
            {
                ValidationDictionary.AddError($"Speech part of name: \"{entity.Name}\"",
                    $"Language \"{entity.LanguageName}\" already contains speech part with given name");
                return false;
            }

            return res;
        }

        public override bool IsReadyToUpdate(SpeechPart entity)
        {
            return IsValid(entity);
        }
    }
}
