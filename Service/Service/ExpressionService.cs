using Data.Models;
using FluentValidation;
using Service.Repository;
using Msg = Commons.ValidationErrorMessages;


namespace Service
{
    public class ExpressionService : ServiceBase<Expression>
    {
        private readonly IExpressionRepository repo;
        private readonly IDictionaryRepository dictRepo;
        private readonly IMeaningRepository meaningRepo;

        public ExpressionService(UnitOfWork uow, AbstractValidator<Expression> v):base(v)
        {
            this.repo = uow.Expressions;
            this.dictRepo = uow.Dictionaries;
            this.meaningRepo = uow.Meanings;
        }

        //TODO test validating Expression with both meaning and dictionary
        public override IValidationDictionary TryAdd(Expression entity)
        {
            if (!IsValid(entity).IsValid) return validationDictionary;

            CheckMeaningAndDictionary(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);
            return validationDictionary;
        }

        public override IValidationDictionary TryUpdate(Expression entity)
        {
            if (!IsValid(entity).IsValid) return validationDictionary;
            //check if there's anything to update
            if (!repo.Exists(e => e.ID == entity.ID))
            {
                validationDictionary.AddError(Msg.DOESNT_EXIST, Msg.DOESNT_EXIST_DESC<Expression>());
                return validationDictionary;
            }

            CheckMeaningAndDictionary(entity);

            if (validationDictionary.IsValid)
                repo.Update(entity);
            return validationDictionary;
        }

        private void CheckMeaningAndDictionary(Expression entity)
        {
            //check if meaning exists
            if (entity.MeaningID != null && !meaningRepo.ExistsByID(entity.MeaningID.Value))
            {
                validationDictionary.AddError(Msg.NOTFOUND<Meaning>(), Msg.NOTFOUND_DESC<Expression, Meaning, int?>(m => m.ID, entity.MeaningID));
            }
            //check if dictionary exists
            if (entity.DictionaryIndex != null && !dictRepo.ExistsByIndex(entity.DictionaryIndex.Value))
            {
                validationDictionary.AddError(Msg.NOTFOUND<Dictionary>(), Msg.NOTFOUND_DESC<Expression, Dictionary, int?>(d => d.Index, entity.DictionaryIndex));
                    
            }
        }
    }
}
