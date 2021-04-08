using Domain.Repository;
using Persistence.Database;
using System;
using Domain.Models;

namespace Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DatabaseContext context;
        private ILanguageRepository langRepo;
        private IDictionaryRepository dictRepo;
        private IWordRepository wordRepo;
        private IEntryRepository entryRepo;
        private IMeaningRepository meaningRepo;
        private IExampleRepository exampleRepo;
        private IFreeExpressionRepository freeExpressionRepo;

        public UnitOfWork() { }

        public UnitOfWork(DatabaseContext context)
        {
            this.context = context;
        }

        public ILanguageRepository Languages
        {
            get
            {
                return langRepo ??= new LanguageRepository(context);
            }
        }

        public IDictionaryRepository Dictionaries
        {
            get
            {
                return dictRepo ??= new DictionaryRepository(context);
            }
        }

        public IWordRepository Words
        {
            get
            {
                return wordRepo ??= new WordRepository(context);
            }
        }

        public IEntryRepository Entries
        {
            get
            {
                return entryRepo ??= new EntryRepository(context);
            }
        }

        public IMeaningRepository Meanings
        {
            get
            {
                return meaningRepo ??= new MeaningRepository(context);
            }
        }

        public IExampleRepository Examples
        {
            get
            {
                return exampleRepo ??= new ExampleRepository(context);
            }
        }

        public IFreeExpressionRepository FreeExpressions
        {
            get
            {
                return freeExpressionRepo ??= new FreeExpressionRepository(context);
            }
        }

        public IRepository<T, K> Generic<T, K>()
        {
            if (typeof(T) == typeof(Language) && typeof(K) == typeof(String))
                return (IRepository<T, K>)Languages;
            
            if (typeof(T) == typeof(Word) && typeof(K) == typeof(int))
                return (IRepository<T, K>)Words;
            
            if (typeof(T) == typeof(Dictionary) && typeof(K) == typeof(int))
                return (IRepository<T, K>)Dictionaries;
            
            if (typeof(T) == typeof(FreeExpression) && typeof(K) == typeof(int))
                return (IRepository<T, K>)FreeExpressions;
            
            if (typeof(T) == typeof(Entry) && typeof(K) == typeof(int))
                return (IRepository<T, K>)Entries;
            
            if (typeof(T) == typeof(Meaning) && typeof(K) == typeof(int))
                return (IRepository<T, K>)Meanings;

            if (typeof(T) == typeof(Example) && typeof(K) == typeof(int))
                return (IRepository<T, K>) Examples;

            throw new ArgumentException(
                $"There is not repository for the type {nameof(T)} whose primary key is {nameof(K)}");
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                context.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
