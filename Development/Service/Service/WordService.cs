using Domain.Repository;
using Domain.Models;
using System;
using Msg = Service.ValidationErrorMessages;
using System.Collections.Generic;
using Service.Service.Abstract;
using Domain.Dto;
using Service.Mapper;
using System.Linq;

namespace Service
{
    public class WordService : ServiceBase, IWordService
    {
        private readonly IWordRepository repo;
        private readonly ILanguageRepository langRepo;
        private ValidationResult validationDictionary;

        public WordService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
        {
            this.repo = uow.Words;
            this.langRepo = uow.Languages;
        }

        public GetWord Get(int id) => Map(repo.GetByID(id));

        public IEnumerable<GetWord> Get(String value) => repo.GetByValue(value, false).Select(Map);

        public ValidationResult Add(CreateWord dto)
        {
            var entity = mapper.Map<CreateWord, Word>(dto);

            this.validationDictionary = ValidationResult.New(entity);

            CheckLanguage(entity);
            if (validationDictionary.IsInvalid)
                return validationDictionary;
            CheckValueAndProperties(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);

            return validationDictionary;
        }

        public ValidationResult Update(UpdateWord dto)
        {
            //check if entity already exists
            if (!repo.ExistsByID(dto.ID))
            {
                this.validationDictionary = ValidationResult.New(dto);
                validationDictionary.AddError(Msg.EntityNotFound(), Msg.ThereIsNothingToUpdate<Word>());
                return validationDictionary;
            }

            var entity = mapper.Map(dto, repo.GetByID(dto.ID));

            this.validationDictionary = ValidationResult.New(entity);

            //We don't check for Language because it cannot be updated.
            CheckValueAndProperties(entity);

            if (validationDictionary.IsValid)
                repo.Update(entity);

            return validationDictionary;
        }

        public ValidationResult Delete(int id)
        {
            var result = ValidationResult.New(id);
            var indb = repo.GetByID(id);

            if(indb == null)
            {
                result.AddError(Msg.EntityNotFound<Word>(), Msg.EntityDoesNotExistByPrimaryKey<Word>());
            }
            else
            {
                repo.Delete(indb);
            }

            return result;
        }

        private void CheckValueAndProperties(Word entity)
        {
            //check if there's a word with same set of WordProperties
            var similar = repo.GetByLanguageAndValue(entity.SourceLanguageName, entity.Value, false);
            foreach (var i in similar)
            {
                if (entity.Properties.SetEquals(i.Properties) && String.Equals(entity.Value, i.Value, StringComparison.OrdinalIgnoreCase))
                {
                    validationDictionary.AddError(Msg.DUPLICATE, Msg.DUPLICATE_WORD_DESC);
                }
            }
        }

        private void CheckLanguage(Word entity)
        {
            //check if language exists
            if (!langRepo.ExistsByName(entity.SourceLanguageName))
            {
                validationDictionary.AddError(Msg.EntityNotFound<Language>(), Msg.EntityDoesNotExistByForeignKey<Word, Language>(l => l.Name, entity.SourceLanguageName));
                return;
            }
        }

        private GetWord Map(Word obj) => mapper.Map<Word, GetWord>(obj);
    }
}
