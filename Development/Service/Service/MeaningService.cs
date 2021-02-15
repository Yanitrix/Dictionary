using Data.Repository;
using Data.Models;
using Msg = Service.ValidationErrorMessages;

namespace Service
{
    public class MeaningService : IMeaningService
    {
        private readonly IMeaningRepository repo;
        private readonly IEntryRepository entryRepo;
        private ValidationResult validationDictionary;

        public MeaningService(IUnitOfWork uow)
        {
            this.repo = uow.Meanings;
            this.entryRepo = uow.Entries;
        }

        public Meaning Get(int id) => repo.GetByID(id);

        public ValidationResult Add(Meaning entity)
        {
            this.validationDictionary = ValidationResult.New(entity);

            CheckEntry(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);

            return validationDictionary;
        }

        public ValidationResult Update(Meaning entity)
        {
            this.validationDictionary = ValidationResult.New(entity);

            if (!repo.ExistsByID(entity.ID))
            {
                validationDictionary.AddError(Msg.EntityNotFound(), Msg.ThereIsNothingToUpdate<Meaning>());
                return validationDictionary;
            }

            if (validationDictionary.IsValid)
                repo.Update(entity);

            return validationDictionary;
        }

        public ValidationResult Delete(int id)
        {
            var result = ValidationResult.New(id);
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
