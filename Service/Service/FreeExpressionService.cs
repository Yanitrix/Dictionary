using Data.Models;
using Data.Repository;
using Msg = Commons.ValidationErrorMessages;

namespace Service
{
    public class FreeExpressionService : IFreeExpressionService
    {
        private readonly IFreeExpressionRepository repo;
        private readonly IDictionaryRepository dictRepo;
        private IValidationDictionary validationDictionary;

        public FreeExpressionService(IUnitOfWork uow)
        {
            repo = uow.FreeExpressions;
            dictRepo = uow.Dictionaries;
        }

        public FreeExpression Get(int id) => repo.GetByID(id);

        public IValidationDictionary Add(FreeExpression entity)
        {
            this.validationDictionary = IValidationDictionary.New();

            CheckDictionary(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);
            return validationDictionary;
        }

        public IValidationDictionary Update(FreeExpression entity)
        {
            this.validationDictionary = IValidationDictionary.New();
            //check if there's anything to update
            if (!repo.Exists(e => e.ID == entity.ID))
            {
                validationDictionary.AddError(Msg.EntityNotFound(), Msg.ThereIsNothingToUpdate<Example>());
                return validationDictionary;
            }

            CheckDictionary(entity);

            if (validationDictionary.IsValid)
                repo.Update(entity);
            return validationDictionary;
        }

        public IValidationDictionary Delete(int id)
        {
            var result = IValidationDictionary.New();
            var indb = repo.GetByID(id);

            if(indb == null)
            {
                result.AddError(Msg.EntityNotFound<FreeExpression>(), Msg.EntityDoesNotExistByPrimaryKey<FreeExpression>());
            }
            else
            {
                repo.Delete(indb);
            }

            return result;
        }

        private void CheckDictionary(FreeExpression entity)
        {
            if (!dictRepo.ExistsByIndex(entity.DictionaryIndex))
            {
                validationDictionary.AddError(Msg.EntityNotFound<Dictionary>(), Msg.EntityDoesNotExistByForeignKey<Example, Meaning>(m => m.ID, entity.DictionaryIndex));
            }
        }
    }
}
