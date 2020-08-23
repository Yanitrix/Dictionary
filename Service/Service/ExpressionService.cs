using Data.Database;
using Data.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Service
{
    public class ExpressionService : ServiceBase<Expression>, IExpressionService
    {

        public ExpressionService(DatabaseContext context, AbstractValidator<Expression> validator):base(context, validator) { }

        public IEnumerable<Expression> GetByTextSubstring(string text)
        {
            return repo.Where(ex => ex.Text.Contains(text));
        }

        public IEnumerable<Expression> GetByTranslationSubstring(string translation)
        {
            return repo.Where(ex => ex.Translation.Contains(translation));
        }

        public override bool IsReadyToAdd(Expression entity)
        {
            if (!IsValid(entity)) return false;

            //check if dictionary or meaning exists

            var meaningIndb = context.Set<Meaning>().Find(entity.MeaningID);
            //TODO maybe check if paremeters are null and then find the one that isn't
            var dictionaryIndb = context.Set<Dictionary>().FirstOrDefault(d => d.Index == entity.DictionaryIndex);

            if (meaningIndb == null && dictionaryIndb == null)
            {
                ValidationDictionary.AddError("Meaning or Dictionary not found", $"Meaning with following ID: \"{entity.MeaningID}\" or " +
                    $"Dictionary with following Index: \"{entity.DictionaryIndex}\" is not present in the database");
            }

            return true;
        }

        public override bool IsReadyToUpdate(Expression entity)
        {
            //checking same things
            return IsReadyToAdd(entity);
        }
    }
}
