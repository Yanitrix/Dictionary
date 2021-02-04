using Data.Database;
using System;

namespace Data.Repository
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
