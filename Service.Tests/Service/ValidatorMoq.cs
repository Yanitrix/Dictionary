using FluentValidation;

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
