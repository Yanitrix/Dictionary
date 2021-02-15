using Domain.Dto;
using Domain.Messages;
using FluentValidation;

namespace Domain.Validation
{
    public class EntryDtoValidator : AbstractValidator<CreateOrUpdateEntry>
    {
        public EntryDtoValidator()
        {
            RuleFor(e => e.DictionaryIndex).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
            RuleFor(e => e.WordID).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
        }
    }
}
