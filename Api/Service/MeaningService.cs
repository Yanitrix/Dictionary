using Api.Service.Abstract;
using Dictionary_MVC.Data;
using Dictionary_MVC.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    public class MeaningService : ServiceBase<Meaning>, IMeaningService
    {

        public MeaningService(DatabaseContext context, AbstractValidator<Meaning> validator):base(context, validator) { }
        
        public Meaning GetByID(int id)
        {
            return repo.Find(id);
        }

        public override bool IsReadyToAdd(Meaning entity)
        {
            if (!IsValid(entity)) return false;

            //check if entry exists

            var entryIndb = context.Set<Entry>().Find(entity.EntryID);

            if (entryIndb == null)
            {
                ValidationDictionary.AddError("Entry not found", $"Entry with given" +
                    $" ID: \"{entity.EntryID}\" does not exist in the database");
                return false;
            }

            return true;
        }

        public override bool IsReadyToUpdate(Meaning entity)
        {
            return IsReadyToAdd(entity);
        }
    }
}
