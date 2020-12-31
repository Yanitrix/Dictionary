using Service.Repository;
using Data.Models;
using Msg = Commons.ValidationErrorMessages;

namespace Service
{
    public class MeaningService : IService<Meaning>
    {
        private readonly IMeaningRepository repo;
        private readonly IEntryRepository entryRepo;
        private IValidationDictionary validationDictionary;

        public MeaningService(IUnitOfWork uow)
        {
            this.repo = uow.Meanings;
            this.entryRepo = uow.Entries;
        }

        public IValidationDictionary TryAdd(Meaning entity)
        {
            this.validationDictionary = IValidationDictionary.New();

            CheckEntry(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);

            return validationDictionary;
        }

        public IValidationDictionary TryUpdate(Meaning entity)
        {
            this.validationDictionary = IValidationDictionary.New();

            if (!repo.ExistsByID(entity.ID))
            {
                validationDictionary.AddError(Msg.DOESNT_EXIST, Msg.DOESNT_EXIST_DESC<Meaning>());
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
                validationDictionary.AddError(Msg.NOTFOUND<Entry>(), Msg.NOTFOUND_DESC<Meaning, Entry, int>(e => e.ID, entity.EntryID));
            }
        }
    }
}
