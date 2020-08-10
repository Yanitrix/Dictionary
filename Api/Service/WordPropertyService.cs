using Api.Service.Abstract;
using Dictionary_MVC.Data;
using Dictionary_MVC.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Api.Service
{
    public class WordPropertyService : ServiceBase<WordProperty>, IWordPropertyService
    {
        public WordPropertyService(DatabaseContext context, AbstractValidator<WordProperty> validator):base(context, validator) { }

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


            //name should be one of the speechpartproperties of the speechpart of the word of this wordproperty
            //entity.Word.SpeechPart.SpeechPartProperties

            var presumableProperties = context.Set<SpeechPart>().Include(sp => sp.Properties)
                .FirstOrDefault(sp => sp.Name == wordIndb.SpeechPartName && sp.LanguageName == wordIndb.SourceLanguageName)
                // if the word was added then also its speech part was, therefore we don't check
                // whether the thing we get from the query is null, it's legit
                .Properties;

            if (presumableProperties.Count == 0) return false; //the collection can actually be empty //well, can it? TODO
            
            if (!presumableProperties.Any(prop => prop.Name == entity.Name && prop.PossibleValues.Contains(entity.Value)))
            {
                ValidationDictionary.AddError("Name or Value invalid", "There is not a single SpeechPartProperty for given " +
                    $"Word and its Language that is of name \"{entity.Name}\" and can have a \"{entity.Value}\" value");
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
