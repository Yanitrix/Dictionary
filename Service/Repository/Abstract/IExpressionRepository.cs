using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    public interface IExpressionRepository : IRepository<Expression>
    {
        public IEnumerable<Expression> GetByTextSubstring(String text);

        public IEnumerable<Expression> GetByTranslationSubstring(String translation);
    }
}
