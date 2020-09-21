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
            //EF had some problems checking with whitespace, and it throws an exception when string is null so we just check and return empty.
            if (text == null || String.IsNullOrWhiteSpace(text)) return Enumerable.Empty<Expression>();
            return repo.Where(ex => ex.Text.Contains(text));
        }

        public IEnumerable<Expression> GetByTranslationSubstring(string translation)
        {
            if (translation == null || String.IsNullOrWhiteSpace(translation)) return Enumerable.Empty<Expression>();
            return repo.Where(ex => ex.Translation.Contains(translation));
        }

    }
}
