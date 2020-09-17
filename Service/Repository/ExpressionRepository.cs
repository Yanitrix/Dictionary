using Data.Database;
using Data.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Service
{
    public class ExpressionRepository : RepositoryBase<Expression>, IExpressionRepository
    {

        public ExpressionRepository(DatabaseContext context):base(context) { }

        public IEnumerable<Expression> GetByTextSubstring(string text)
        {
            return repo.Where(ex => ex.Text.Contains(text));
        }

        public IEnumerable<Expression> GetByTranslationSubstring(string translation)
        {
            return repo.Where(ex => ex.Translation.Contains(translation));
        }

    }
}
