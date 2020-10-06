using Api.Service;
using Data.Models;
using FluentValidation;
using Service.Service.Abstract;
using System;
using System.Text;


//TODO ogarnąć namespaces
namespace Service.Service
{
    public class DictionaryService : ServiceBase<Dictionary>
    {
        private readonly ILanguageRepository langRepo;
        private readonly IDictionaryRepository repo;

        public DictionaryService(ILanguageRepository langRepo, IDictionaryRepository repo, AbstractValidator<Dictionary> validator)
            :base(validator)
        {
            this.langRepo = langRepo;
            this.repo = repo;
        }

        public override IValidationDictionary TryAdd(Dictionary entity)
        {
            if (!IsValid(entity).IsValid) return validationDictionary;

            //check if dict already exists
            if (repo.ExistsByLanguages(entity.LanguageInName, entity.LanguageOutName))
            {
                validationDictionary.AddError("Duplicate", "A Dictionary of same Languages already exist in the database");
                return validationDictionary;
            }
            //check if langueges exist
            if (!langRepo.ExistsByName(entity.LanguageInName) || !langRepo.ExistsByName(entity.LanguageOutName))
            {
                validationDictionary.AddError("Language not found", $"Language: \"{entity.LanguageInName}\" or \"{entity.LanguageOutName}\"" +
                    " does not exist in the database. Create them before posting a dictionary");
                return validationDictionary;
            }

            if (validationDictionary.IsValid)
                repo.Create(entity);

            return validationDictionary;
        }

        public override IValidationDictionary TryUpdate(Dictionary entity)
        {
            validationDictionary = new ValidationDictionary();
            
            validationDictionary.AddError("Entity cannot be updated",
                "LanguageIn and LanguageOut properties of a Dictionary cannot be updated. If you want to add Entries or FreeExpressions to a Dictionary, post them on their respective endpoints");
            return validationDictionary;
        }
    }
}
