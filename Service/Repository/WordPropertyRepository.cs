using Data.Database;
using Data.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Api.Service
{
    public class WordPropertyRepository : RepositoryBase<WordProperty>, IWordPropertyRepository
    {
        public WordPropertyRepository(DatabaseContext context, AbstractValidator<WordProperty> validator):base(context, validator) { }

        public WordProperty GetByID(int id)
        {
            return repo.Find(id);
        }

        public override bool IsReadyToAdd(WordProperty entity)
        {
            if (!IsValid(entity)) return false;

            //check if word exists

            var wordRepo = context.Set<Word>();
            var wordIndb = wordRepo.Find(entity.WordID);
            if(wordIndb == null)
            {
                ValidationDictionary.AddError("Word not found", $"Word with given id: \"{entity.WordID}\" doesn't exist in the database");
                return false;
            }

            return true;
        }

        public override bool IsReadyToUpdate(WordProperty entity)
        {
            //checking same things
            return IsReadyToAdd(entity);
        }
    }
}
