using Data.Database;
using Data.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    public class MeaningRepository : RepositoryBase<Meaning>, IMeaningRepository
    {

        public MeaningRepository(DatabaseContext context):base(context) { }
        
        public Meaning GetByID(int id)
        {
            return repo.Find(id);
        }

        public override Meaning Delete(Meaning entity) //Examples must be included
        {
            context.Set<Expression>().RemoveRange(entity.Examples);
            base.Delete(entity);
            return entity;
        }

    }
}
