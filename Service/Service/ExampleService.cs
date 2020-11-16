using Data.Models;
using FluentValidation;
using Service.Repository;
using Msg = Commons.ValidationErrorMessages;


namespace Service
{
    public class ExampleService : ServiceBase<Example>
    {
        private readonly IExampleRepository repo;
        private readonly IMeaningRepository meaningRepo;

        public ExampleService(IUnitOfWork uow, AbstractValidator<Example> v) : base(v)
        {
            this.repo = uow.Examples;
            this.meaningRepo = uow.Meanings;
        }

        public override IValidationDictionary TryAdd(Example entity)
        {
            if (!IsValid(entity).IsValid) return validationDictionary;

            CheckMeaning(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);
            return validationDictionary;
        }

        public override IValidationDictionary TryUpdate(Example entity)
        {
            if (!IsValid(entity).IsValid) return validationDictionary;
            //check if there's anything to update
            if (!repo.Exists(e => e.ID == entity.ID))
            {
                validationDictionary.AddError(Msg.DOESNT_EXIST, Msg.DOESNT_EXIST_DESC<Example>());
                return validationDictionary;
            }

            CheckMeaning(entity);

            if (validationDictionary.IsValid)
                repo.Update(entity);
            return validationDictionary;
        }

        private void CheckMeaning(Example entity)
        {
            //check if meaning exists
            if (!meaningRepo.ExistsByID(entity.MeaningID))
            {
                validationDictionary.AddError(Msg.NOTFOUND<Meaning>(), Msg.NOTFOUND_DESC<Example, Meaning, int>(m => m.ID, entity.MeaningID));
            }
        }
    }
}
