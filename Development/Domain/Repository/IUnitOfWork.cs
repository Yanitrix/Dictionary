using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Domain.Repository
{
    public interface IUnitOfWork
    {

        public ILanguageRepository Languages { get; }
        public IWordRepository Words { get; }
        public IDictionaryRepository Dictionaries { get; }
        public IEntryRepository Entries { get; }
        public IMeaningRepository Meanings { get; }
        public IExampleRepository Examples { get; }
        public IFreeExpressionRepository FreeExpressions { get; }

        public IRepository<T, K> Generic<T, K>();
    }
}
