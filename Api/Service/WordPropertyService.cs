using Api.Service.Abstract;
using Dictionary_MVC.Data;
using Dictionary_MVC.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

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

            //name should be one of the speechpartproperties of the speechpart of the word of this wordproperty
            //entity.Word.SpeechPart.SpeechPartProperties

            return false;
        }

        public override bool IsReadyToUpdate(WordProperty entity)
        {
            throw new NotImplementedException();
        }
    }
}
