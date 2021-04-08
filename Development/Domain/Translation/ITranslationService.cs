using System;

namespace Domain.Translation
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

        TranslationResponse Translate(String dictionary, String query);

        BidirectionalTranslationResponse TranslateBidir(String dictionaries, String query);
    }
}
