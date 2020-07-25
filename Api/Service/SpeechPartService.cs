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
        public SpeechPartService(DatabaseContext context, AbstractValidator<SpeechPart> validator) : base(context, validator)
        {
        }

        public SpeechPart GetByIndex(int index)
        {
            return repo.Where(x => x.Index == index).SingleOrDefault();
        }

        public SpeechPart GetByNameAndLanguage(string languageName, string name)
        {
            //return repo.SingleOrDefault(x => String.Equals(x.LanguageName, languageName, StringComparison.OrdinalIgnoreCase) && x.Name == name);
            return repo.Find(languageName, name);
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
