using Dictionary_MVC.Data;
using Dictionary_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{

    public class LanguageService : ServiceBase<Language>
    {
        public LanguageService(DatabaseContext context) : base(context) { }

        public override bool IsValid(Language entity)
        {
            //custom validation
            return false;
        }

        public override Language Create(Language entity)
        {
            throw new NotImplementedException();
        }

        public override Language GetOne(Func<Language, bool> condition)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Language> Get(Func<Language, bool> condition)
        {
            throw new NotImplementedException();
        }

        public override Language Update(Language entity)
        {
            throw new NotImplementedException();
        }

        public override Language Delete(Language entity)
        {
            throw new NotImplementedException();
        }
    }
}
