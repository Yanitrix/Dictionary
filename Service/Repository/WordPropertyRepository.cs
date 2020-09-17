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
        public WordPropertyRepository(DatabaseContext context):base(context) { }

        public WordProperty GetByID(int id)
        {
            return repo.Find(id);
        }

    }
}
