using Domain.Models;
using Domain.Repository;
using Persistance.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistance.Repository
{
    public abstract class ExpressionRepositoryBase<T> : RepositoryBase<T>, IExpressionRepositoryBase<T> where T : Expression
    {
        protected Func<IQueryable<T>, IOrderedQueryable<T>> orderFunction = (src) => src.OrderBy(e => e.Text).ThenBy(e => e.Translation);

        protected ExpressionRepositoryBase(DatabaseContext context) : base(context) { }

        public abstract IEnumerable<T> GetByDictionaryAndTextSubstring(int dictionaryIndex, string textSubstring);

        public abstract IEnumerable<T> GetByDictionaryAndTranslationSubstring(int dictionaryIndex, string translationSubstring);

        public T GetByID(int id)
        {
            return GetOne(x => x.ID == id);
        }

        public IEnumerable<T> GetByTextSubstring(string textSubstring)
        {
            if (String.IsNullOrEmpty(textSubstring) || String.IsNullOrWhiteSpace(textSubstring)) return Enumerable.Empty<T>();
            return Get(f => f.Text.Contains(textSubstring), x => x, orderFunction);
        }

        public IEnumerable<T> GetByTranslationSubstring(string translationSubstring)
        {
            if (String.IsNullOrEmpty(translationSubstring) || String.IsNullOrWhiteSpace(translationSubstring)) return Enumerable.Empty<T>();
            return Get(f => f.Translation.Contains(translationSubstring), x => x, orderFunction);
        }
    }
}
