using Data.Database;
using Data.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    public class MeaningRepository : RepositoryBase<Meaning>, IMeaningRepository
    {

        public MeaningRepository(DatabaseContext context, AbstractValidator<Meaning> validator):base(context, validator) { }
        
        public Meaning GetByID(int id)
        {
            return repo.Find(id);
        }

        public override Meaning Delete(Meaning entity) //Examples must be included
        {
            context.Set<Expression>().RemoveRange(entity.Examples);
            base.Delete(entity);
            return entity;
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
