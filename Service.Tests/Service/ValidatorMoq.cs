using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Tests.Service
{
    public class ValidatorMoq<T> : AbstractValidator<T>
    {
    }

    public static class VMoq
    {
        public static ValidatorMoq<T> Instance<T>()
        {
            return new ValidatorMoq<T>();
        }
    }

}
