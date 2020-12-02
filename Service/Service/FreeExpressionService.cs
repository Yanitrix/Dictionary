using Data.Models;
using FluentValidation;
using Service.Repository;
using Msg = Commons.ValidationErrorMessages;

namespace Service
{
    public class FreeExpressionService : ServiceBase<FreeExpression>
    {
        private readonly IFreeExpressionRepository repo;
        private readonly IDictionaryRepository dictRepo;

        public FreeExpressionService(IUnitOfWork uow, AbstractValidator<FreeExpression> v) : base(v) {

            repo = uow.FreeExpressions;
            dictRepo = uow.Dictionaries;
        }

        public override IValidationDictionary TryAdd(FreeExpression entity)
        {
            if (!IsValid(entity).IsValid) return validationDictionary;

            CheckDictionary(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);
            return validationDictionary;
        }

        public override IValidationDictionary TryUpdate(FreeExpression entity)
        {
            if (!IsValid(entity).IsValid) return validationDictionary;
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
                validationDictionary.AddError(Msg.NOTFOUND<Dictionary>(), Msg.NOTFOUND_DESC<Example, Meaning, int>(m => m.ID, entity.DictionaryIndex));
            }
        }
    }
}
