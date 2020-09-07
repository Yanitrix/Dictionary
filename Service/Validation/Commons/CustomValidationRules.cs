using Commons;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Api.Service.Validation
{
    public static class CustomValidationRules
    {
        public static IRuleBuilderOptions<T, string> DoesNotMatch<T>(this IRuleBuilderOptions<T, string> builder, string expression)
        {
            Regex regex = new Regex(expression, RegexOptions.None, TimeSpan.FromSeconds(3.0));
            return builder.Must(instance => !regex.IsMatch(instance));
        }

        public static IRuleBuilderOptions<T, string> NoDigitsNoSpaces<T>(this IRuleBuilderOptions<T, string> builder)
        {
            return builder.NoDigits().DoesNotMatch(RegexConstants.ANY_SPACE).WithMessage(MessageConstants.NO_SPACE_MESSAGE);
        }

        public static IRuleBuilderOptions<T, string> NoDigits<T>(this IRuleBuilderOptions<T, string> builder)
        {
            return builder.DoesNotMatch(RegexConstants.ANY_DIGIT).WithMessage(MessageConstants.NO_DIGIT_MESSAGE);
        }
    }
}
