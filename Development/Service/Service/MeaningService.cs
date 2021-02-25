using Domain.Repository;
using Domain.Models;
using Msg = Service.ValidationErrorMessages;
using Service.Service.Abstract;
using Service.Mapper;
using Domain.Dto;

namespace Service
{
    public class MeaningService : ServiceBase, IMeaningService
    {
        private readonly IMeaningRepository repo;
        private readonly IEntryRepository entryRepo;
        private ValidationResult validationDictionary;

        public MeaningService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
        {
            this.repo = uow.Meanings;
            this.entryRepo = uow.Entries;
        }

        public GetMeaning Get(int id) => Map(repo.GetByID(id));

        public ValidationResult Add(CreateMeaning dto)
        {
            var entity = mapper.Map<CreateMeaning, Meaning>(dto);

            this.validationDictionary = ValidationResult.New(entity);

            CheckEntry(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);

            return validationDictionary;
        }

        public ValidationResult Update(UpdateMeaning dto)
        {
            var entity = mapper.Map<UpdateMeaning, Meaning>(dto);

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

        private GetMeaning Map(Meaning obj) => mapper.Map<Meaning, GetMeaning>(obj);
    }
}
