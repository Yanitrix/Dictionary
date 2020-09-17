using Data.Dto;
using Data.Database;
using Data.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Service
{
    
    public class LanguageRepository : RepositoryBase<Language>, ILanguageRepository
    {
        public LanguageRepository(DatabaseContext context):base(context) { }

        public Language GetByName(String name)
        {
            return repo.Find(name);
        }

    }
}
