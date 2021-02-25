﻿using Domain.Dto;
using Domain.Models;
using Domain.Repository;
using Service.Mapper;
using Service.Service.Abstract;
using Msg = Service.ValidationErrorMessages;

namespace Service
{
    public class FreeExpressionService : ServiceBase, IFreeExpressionService
    {
        private readonly IFreeExpressionRepository repo;
        private readonly IDictionaryRepository dictRepo;
        private ValidationResult validationDictionary;

        public FreeExpressionService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
        {
            repo = uow.FreeExpressions;
            dictRepo = uow.Dictionaries;
        }

        public GetFreeExpression Get(int id) => Map(repo.GetByID(id));

        public ValidationResult Add(CreateFreeExpression dto)
        {
            var entity = mapper.Map<CreateFreeExpression, FreeExpression>(dto);

            this.validationDictionary = ValidationResult.New(entity);

            CheckDictionary(entity);

            if (validationDictionary.IsValid)
                repo.Create(entity);
            return validationDictionary;
        }

        public ValidationResult Update(UpdateFreeExpression dto)
        {
            var entity = mapper.Map<UpdateFreeExpression, FreeExpression>(dto);

            this.validationDictionary = ValidationResult.New(entity);
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

        public ValidationResult Delete(int id)
        {
            var result = ValidationResult.New(id);
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

        private GetFreeExpression Map(FreeExpression obj) => mapper.Map<FreeExpression, GetFreeExpression>(obj);
    }
}
