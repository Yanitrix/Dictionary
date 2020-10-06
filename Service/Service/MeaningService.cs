using Service.Repository;
using Data.Models;
using FluentValidation;

namespace Service
{
    public class MeaningService : ServiceBase<Meaning>
    {
        private readonly IMeaningRepository repo;
        private readonly IEntryRepository entryRepo;

        public MeaningService(UnitOfWork uow, AbstractValidator<Meaning> _v)
            :base(_v)
        {
            this.repo = uow.Meanings;
            this.entryRepo = uow.Entries;
        }

        public override IValidationDictionary TryAdd(Meaning entity)
        {
            if (!IsValid(entity).IsValid) return validationDictionary;

            CheckEntry(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);

            return validationDictionary;
        }

        public override IValidationDictionary TryUpdate(Meaning entity)
        {
            if (!IsValid(entity).IsValid) return validationDictionary;

            if (!repo.ExistsByID(entity.ID))
            {
                validationDictionary.AddError("Entity does not exist", "Meaning with given primary key does not exist in the database. There is nothing to update");
                return validationDictionary;
            }

            CheckEntry(entity);

            if (validationDictionary.IsValid)
                repo.Update(entity);

            return validationDictionary;
        }

        private void CheckEntry(Meaning entity)
        {
            //only checking if entry exists
            if (!entryRepo.ExistsByID(entity.EntryID))
            {
                validationDictionary.AddError("Entry not found", "Entry with given primary key does not exist in the database");
            }
        }
    }
}
