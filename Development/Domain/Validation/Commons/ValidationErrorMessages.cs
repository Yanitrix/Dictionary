﻿using System;
using System.Linq.Expressions;

namespace Domain
{
    public static class ValidationErrorMessages
    {
        public const String DUPLICATE_LANGUAGE_DESC = "Language with same name already exists in the database. The case is ignored";
        public const String DUPLICATE_DICTIONARY_DESC = "Dictionary of same Languages already exist in the database. The case is ignored";
        public const String DUPLICATE_ENTRY_DESC = "An Entry for given Word and Dictionary already exists. If you want to add another Meaning, post it on its respective endpoint";

        public const String CANNOT_UPDATE_ENTRY_DESC = "Properties of given Entry cannot be updated because the Entry already includes Meeanings. If you want to add Meanings to the Entry, post them on their respective endpoint.";

        public const String LANGUAGES_NOT_MATCH_DESC = "SourceLanguage of the Word is different than LanguageIn of the Dictionary. The case is NOT ignored";

        public static String EntityDoesNotExistByPrimaryKey<T>() => $"{typeof(T).Name} with given primary key does not exist.";
        public static String ThereIsNothingToUpdate<T>() => EntityDoesNotExistByPrimaryKey<T>() + " There is nothing to update";
        public static String EntityDoesNotExistByForeignKey<A, B>(Expression<Func<B, object>> pkeyEx, object pkeyValue)
        {
            return pkeyEx.Body switch
            {
                MemberExpression m => $"{typeof(B).Name} with given {m.Member.Name}: {pkeyValue} was not found in the database. Create it before posting a(n) {typeof(A).Name}",
                UnaryExpression u when u.Operand is MemberExpression m => $"{typeof(B).Name} with given {m.Member.Name}: {pkeyValue} was not found in the database. Create it before posting a(n) {typeof(A).Name}",
                _ => "null",
            };
        }
        public static String LanguagesNotFoundDesc(String langIn, String langOut) => $"Language: \"{langIn}\" or \"{langOut}\" does not exist in the database. Create them before posting a Dictionary";
    }
}
