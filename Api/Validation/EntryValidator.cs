using Commons;
using Dictionary_MVC.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Validation
{
    public class EntryValidator : AbstractValidator<Entry>
    {
        public EntryValidator()
        {
            RuleFor(e => e.ID).Empty().WithMessage(MessageConstants.EMPTY_ID);

            RuleFor(e => e.DictionaryIndex).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
            RuleFor(e => e.WordID).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
            RuleFor(e => e.Meanings).Empty().WithMessage("The collection must be initially empty. New Meanings will be added when posting a Meaning object with a correct EntryID");
            //RuleForEach(e => e.Meanings).SetValidator(new MeaningValidator()); //TODO as the message says, Entry is posted empty, then correct meanings are assigned according to EntryID while posting a meaning object
        }
    }
}
