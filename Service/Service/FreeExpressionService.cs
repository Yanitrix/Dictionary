using Data.Models;
using Service.Repository;
using Msg = Commons.ValidationErrorMessages;

namespace Service
{
    public class FreeExpressionService : IService<FreeExpression>
    {
        private readonly IFreeExpressionRepository repo;
        private readonly IDictionaryRepository dictRepo;
        private IValidationDictionary validationDictionary;

        public FreeExpressionService(IUnitOfWork uow)
        {

            repo = uow.FreeExpressions;
            dictRepo = uow.Dictionaries;
        }

        public IValidationDictionary TryAdd(FreeExpression entity)
        {
            this.validationDictionary = IValidationDictionary.New();

            CheckDictionary(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);
            return validationDictionary;
        }

        public IValidationDictionary TryUpdate(FreeExpression entity)
        {
            this.validationDictionary = IValidationDictionary.New();
            //check if there's anything to update
            if (!repo.Exists(e => e.ID == entity.ID))
            {
                validationDictionary.AddError(Msg.DOESNT_EXIST, Msg.DOESNT_EXIST_DESC<Example>());
                return validationDictionary;
            }

            CheckDictionary(entity);

            if (validationDictionary.IsValid)
                repo.Update(entity);
            return validationDictionary;
        }

        private void CheckDictionary(FreeExpression entity)
        {
            if (!dictRepo.ExistsByIndex(entity.DictionaryIndex))
            {
                validationDictionary.AddError(Msg.NOTFOUND<Dictionary>(), Msg.NOTFOUND_DESC<Example, Meaning>(m => m.ID, entity.DictionaryIndex));
            }
        }
    }
}
