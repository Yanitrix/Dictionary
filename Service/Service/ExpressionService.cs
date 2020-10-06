using Data.Models;
using FluentValidation;
using Service.Repository;

namespace Service
{
    public class ExpressionService : ServiceBase<Expression>
    {
        private readonly IExpressionRepository repo;
        private readonly IDictionaryRepository dictRepo;
        private readonly IMeaningRepository meaningRepo;

        public ExpressionService(IExpressionRepository repo, IDictionaryRepository dictRepo,
            IMeaningRepository meaningRepo, AbstractValidator<Expression> v):base(v)
        {
            this.repo = repo;
            this.dictRepo = dictRepo;
            this.meaningRepo = meaningRepo;
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
                validationDictionary.AddError("Entity does not exist", "Expression with given primary key does not exist. There's nothing to update");
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
                validationDictionary.AddError("Meaning not found",
                    $"Meaning with ID: {entity.MeaningID} does not exist in the database. Create it before posting an Expression.");
            }
            //check if dictionary exists
            if (entity.DictionaryIndex != null && !dictRepo.ExistsByIndex(entity.DictionaryIndex.Value))
            {
                validationDictionary.AddError("Dictionary not found",
                    $"Dctionary with Index: {entity.DictionaryIndex} does not exist in the database. Create it before Posting an Expression.");
            }
        }
    }
}
