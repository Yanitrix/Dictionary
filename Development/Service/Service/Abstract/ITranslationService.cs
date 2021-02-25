using Domain.Models;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface ITranslationService
    {
        /// <summary>
        /// If bidirectional is true then at least one of the dictionaries has to exist in order to return true.
        /// </summary>
        /// <param name="languageIn"></param>
        /// <param name="languageOut"></param>
        /// <param name="bidirectional"></param>
        bool EnsureDictionaryExists(String languageIn, String languageOut, bool bidirectional = false);

        IEnumerable<Entry> GetMatchingEntries(String languageIn, String languageOut, String query);

        (IEnumerable<Entry> unidirectional, IEnumerable<Entry> opposite) GetMatchingEntriesBidir(String languageIn, String languageOut, String query);

        IEnumerable<FreeExpression> GetMatchingExpressions(String languageIn, String languageOut, String query);

        (IEnumerable<FreeExpression> unidirectional, IEnumerable<FreeExpression> opposite) GetMatchingExpressionsBidir(String languageIn, String languageOut, String query);
    }
}
