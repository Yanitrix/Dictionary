﻿using Data.Dto;
using Data.Database;
using Data.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Api.Service
{
    
    public class LanguageRepository : RepositoryBase<Language>, ILanguageRepository
    {
        private readonly Func<IQueryable<Language>, IIncludableQueryable<Language, object>> includeQuery = 
            (languages => languages.Include(l => l.Words).ThenInclude(w => w.Properties));

        public LanguageRepository(DatabaseContext context):base(context) { }

        public Language GetByName(String name)
        {
            return repo.Find(name);
        }

        public Language GetByNameWithWords(String name)
        {
            return GetOne(l => l.Name == name, null, includeQuery);
        }

        public Language GetOneWithWords(Expression<Func<Language, bool>> condition)
        {
            return GetOne(condition, null, includeQuery);
        }
    }
}
