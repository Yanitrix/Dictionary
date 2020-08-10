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
    public class SpeechPartPropertyService : ServiceBase<SpeechPartProperty>, ISpeechPartPropertyService
    {

        public SpeechPartPropertyService(DatabaseContext context, AbstractValidator<SpeechPartProperty> validator):base(context, validator) { }

        public SpeechPartProperty GetByID(int id)
        {
            return repo.Find(id);
        }

        public override bool IsReadyToAdd(SpeechPartProperty entity)
        {
            if (!IsValid(entity)) return false;

            //check if db contains speechpart
            var speechPartIndb = context.Set<SpeechPart>().FirstOrDefault(sp => sp.Index == entity.SpeechPartIndex);
            if (speechPartIndb == null)
            {
                ValidationDictionary.AddError("Speech part not found", $"Speech part with given " +
                    $"index: \"{entity.SpeechPartIndex}\" doesn't exist in the database");
                return false;
            }

            return true;
        }

        public override bool IsReadyToUpdate(SpeechPartProperty entity)
        {
            //same checks, will do the same
            return IsReadyToAdd(entity);
        }
    }
}
