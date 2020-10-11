using Service.Repository;
using Data.Models;
using FluentValidation;
using Msg = Commons.ValidationErrorMessages;

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
