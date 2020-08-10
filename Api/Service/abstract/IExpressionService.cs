using Dictionary_MVC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service.Abstract
{
    public interface IExpressionService : IService<Expression>
    {
        public IEnumerable<Expression> GetByTextSubstring(String text);

        public IEnumerable<Expression> GetByTranslationSubstring(String translation);
    }
}
