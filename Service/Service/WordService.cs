using Api.Service;
using Data.Models;
using FluentValidation;
using Service.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Service
{
    public class WordService : ServiceBase<Word>
    {
        private readonly IWordRepository repo;
        private readonly ILanguageRepository langRepo;


        public WordService(IWordRepository repo, ILanguageRepository langRepo, AbstractValidator<Word> validator)
            :base(validator)
        {
            this.repo = repo;
            this.langRepo = langRepo;
        }

        public override IValidationDictionary TryAdd(Word entity)
        {
            if (!IsValid(entity).IsValid) return validationDictionary;

            CheckLanguageAndProperties(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);

            return validationDictionary;
        }

        public override IValidationDictionary TryUpdate(Word entity)
        {
            if (!IsValid(entity).IsValid) return validationDictionary;

            //check if entity already exists
            if (!repo.ExistsByID(entity.ID))
            {
                validationDictionary.AddError("Entity does not exist", "No Word matching entity's primary key was found in the database. " +
                    "There is nothing that can be updated");
                return validationDictionary;
            }

            //now, same checks as for adding
            CheckLanguageAndProperties(entity);

            if (validationDictionary.IsValid)
                repo.Update(entity);

            return validationDictionary;
        }

        private void CheckLanguageAndProperties(Word entity)
        {
            //check if language exists
            if (!langRepo.ExistsByName(entity.SourceLanguageName))
            {
                validationDictionary.AddError("Language not found",
                    $"Language by the name of: \"{entity.SourceLanguage}\" does not exist in the database. Create it before adding a word");
                return;
            }

            //check if there's a word with same set of WordProperties
            //TODO implement equals check on WordProperty and its HashSet<String> of values
            var similar = repo.GetByLanguageAndValue(entity.SourceLanguageName, entity.Value);
            foreach (var i in similar)
            {
                if (entity.Properties.SetEquals(i.Properties) && String.Equals(entity.Value, i.Value, StringComparison.OrdinalIgnoreCase))
                {
                    validationDictionary.AddError("Duplicate",
                        "There's already at least one Word in the database with same Value, same SourceLanguageName and same set of WordProperties");
                }
            }
        }
    }
}
