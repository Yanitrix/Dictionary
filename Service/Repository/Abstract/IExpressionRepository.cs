using Data.Models;
using System;
using System.Collections.Generic;

namespace Service.Repository
{
    public interface IExpressionRepository : IRepository<Expression>
    {
        public IEnumerable<Expression> GetByTextSubstring(String text);

        public IEnumerable<Expression> GetByTranslationSubstring(String translation);
    }
}
