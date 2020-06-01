﻿using Dictionary_MVC.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Service
{
    public abstract class ServiceBase<T> : IService<T> where T : class
    {
        protected readonly DatabaseContext context;
        protected readonly DbSet<T> repo;

        public IValidationDictionary ValidationDictionary { get; set; }

        public ServiceBase(DatabaseContext context)
        {
            this.context = context;
            repo = context.Set<T>();
        }

        public abstract bool IsValid(T entity);

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

        public virtual T GetOne(Func<T, bool> condition)
        {
            return context.Set<T>().SingleOrDefault(condition);
        }

        public virtual IEnumerable<T> Get(Func<T, bool> condition)
        {
            return context.Set<T>().Where(condition).ToList();

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