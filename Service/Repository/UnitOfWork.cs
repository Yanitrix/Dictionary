using Data.Database;
using System;

namespace Service.Repository
{
    //virtual properties and zero constructor because of moq
    public class UnitOfWork : IDisposable
    {
        private readonly DatabaseContext context;
        private ILanguageRepository langRepo;
        private IDictionaryRepository dictRepo;
        private IWordRepository wordRepo;
        private IEntryRepository entryRepo;
        private IMeaningRepository meaningRepo;
        private IExpressionRepository expressionRepo;

        public UnitOfWork()
        {

        }

        public UnitOfWork(DatabaseContext context)
        {
            this.context = context;
        }

        public virtual ILanguageRepository Languages
        {
            get
            {
                return langRepo ??= new LanguageRepository(context);
            }
        }

        public virtual IDictionaryRepository Dictionaries
        {
            get
            {
                return dictRepo ??= new DictionaryRepository(context);
            }
        }

        public virtual IWordRepository Words
        {
            get
            {
                return wordRepo ??= new WordRepository(context);
            }
        }

        public virtual IEntryRepository Entries
        {
            get
            {
                return entryRepo ??= new EntryRepository(context);
            }
        }

        public virtual IMeaningRepository Meanings
        {
            get
            {
                return meaningRepo ??= new MeaningRepository(context);
            }
        }

        public virtual IExpressionRepository Expressions
        {
            get
            {
                return expressionRepo ??= new ExpressionRepository(context);
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
