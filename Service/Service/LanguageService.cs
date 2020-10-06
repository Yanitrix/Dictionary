using Service.Repository;
using Data.Models;
using FluentValidation;

namespace Service
{
    public class LanguageService : ServiceBase<Language>
    {
        private readonly ILanguageRepository repo;

        public LanguageService(AbstractValidator<Language> validator, ILanguageRepository repo):base(validator)
        {
            this.repo = repo;
        }

        public override IValidationDictionary TryAdd(Language entity)
        {
            if (!IsValid(entity).IsValid) return validationDictionary;

            if (repo.ExistsByName(entity.Name))
            {
                validationDictionary.AddError("Duplicate", $"Language with name: \"{entity.Name}\" already exists in the database. The case is ignored");
            }

            if (validationDictionary.IsValid)
            {
                repo.Create(entity);
            }

            return validationDictionary;
        }

        public override IValidationDictionary TryUpdate(Language entity)
        {
            validationDictionary = new ValidationDictionary();

            validationDictionary.AddError("Entity cannot be updated",
                "Name property cannot be changed. If you want to add words to a language, post words with proper language name");

            return validationDictionary;
        }
    }
}
