using Dictionary_MVC.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Api.Service
{
    public abstract class ServiceBase<T> : IService<T> where T : class
    {
        protected readonly AbstractValidator<T> validator;
        protected readonly DatabaseContext context;
        protected readonly DbSet<T> repo;

        public IValidationDictionary ValidationDictionary { get; set; }

        public ServiceBase(DatabaseContext context, AbstractValidator<T> validator)
        {
            this.context = context;
            this.validator = validator;
            repo = context.Set<T>();
        }

        public virtual bool IsValid(T entity)
        {
            var result = validator.Validate(entity);
            foreach (var err in result.Errors) ValidationDictionary.AddError(err.PropertyName, err.ErrorMessage);

            return result.IsValid;
        }

        public abstract bool IsReadyToAdd(T entity);

        public abstract bool IsReadyToUpdate(T entity);

        public IEnumerable<T> All()
        {
            return repo.ToList();
        }

        public virtual T Create(T entity)
        {
            repo.Add(entity);
            context.SaveChanges();
            return entity;
        }

        public virtual T GetOne(Expression<Func<T, bool>> condition)
        {
            return repo.SingleOrDefault(condition);
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> condition)
        {
            return repo.Where(condition).ToList();
        }

        public virtual T Update(T entity)
        {
            repo.Update(entity);
            context.SaveChanges();
            return entity;
        }

        public virtual T Delete(T entity)
        {
            repo.Remove(entity);
            context.SaveChanges();
            return entity;
        }
    }
}