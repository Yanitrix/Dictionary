using System;

namespace Commons
{
    public static class RegexConstants
    {
        [Obsolete]
        public const String ALPHA_REGEX = "";
        [Obsolete]
        public const String ONE_WORD_REGEX = "^[a-zA-Z]+$";

        public const String ANY_DIGIT = "\\d";
        public const String ANY_SPACE = "\\s";
    }

    public static class MessageConstants
    {
        public const String EMPTY_ID = "The ID field must be empty";
        public const String EMPTY_INDEX = "The Index field must be empty";
        public const String EMPTY = "Must be empty";
        public const String NOT_EMPTY = "Cannot be empty";

        public const String NO_DIGIT_MESSAGE = "The literal cannot contain any digits";
        public const String NO_SPACE_MESSAGE = "The literal cannot contain any space characets";
    }
}
