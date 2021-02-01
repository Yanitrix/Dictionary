﻿using Service.Repository;
using Data.Models;
using Msg = Commons.ValidationErrorMessages;

namespace Service
{
    public class MeaningService : IMeaningService
    {
        private readonly IMeaningRepository repo;
        private readonly IEntryRepository entryRepo;
        private IValidationDictionary validationDictionary;

        public MeaningService(IUnitOfWork uow)
        {
            this.repo = uow.Meanings;
            this.entryRepo = uow.Entries;
        }

        public Meaning Get(int id) => repo.GetByID(id);

        public IValidationDictionary Add(Meaning entity)
        {
            this.validationDictionary = IValidationDictionary.New();

            CheckEntry(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);

            return validationDictionary;
        }

        public IValidationDictionary Update(Meaning entity)
        {
            this.validationDictionary = IValidationDictionary.New();

            if (!repo.ExistsByID(entity.ID))
            {
                validationDictionary.AddError(Msg.EntityNotFound(), Msg.ThereIsNothingToUpdate<Meaning>());
                return validationDictionary;
            }

            if (validationDictionary.IsValid)
                repo.Update(entity);

            return validationDictionary;
        }

        public IValidationDictionary Delete(int id)
        {
            var result = IValidationDictionary.New();
            var indb = repo.GetByID(id);

            if (indb == null)
            {
                result.AddError(Msg.EntityNotFound<Meaning>(), Msg.EntityDoesNotExistByPrimaryKey<Meaning>());
            }
            else
            {
                repo.Delete(indb);
            }

            return result;
        }

        private void CheckEntry(Meaning entity)
        {
            //only checking if entry exists
            if (!entryRepo.ExistsByID(entity.EntryID))
            {
                validationDictionary.AddError(Msg.EntityNotFound<Entry>(), Msg.EntityDoesNotExistByForeignKey<Meaning, Entry>(e => e.ID, entity.EntryID));
            }
        }
    }
}
