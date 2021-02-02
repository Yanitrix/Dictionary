using Data.Models;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface ITranslationService
    {
        IEnumerable<Entry> GetMatchingEntries(String languageIn, String languageOut, String query);

        (IEnumerable<Entry> unidirectional, IEnumerable<Entry> opposite) GetMatchingEntriesBidir(String languageIn, String languageOut, String query);

        IEnumerable<FreeExpression> GetMatchingExpressions(String languageIn, String languageOut, String query);

        (IEnumerable<FreeExpression> unidirectional, IEnumerable<FreeExpression> opposite) GetMatchingExpressionsBidir(String languageIn, String languageOut, String query);

        bool EnsureDictionaryExists(String languageIn, String languageOut, bool bidirectional = false);
    }
}
