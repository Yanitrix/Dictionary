using System;
using Domain.Dto;
using Domain.Messages;
using Domain.Models;
using Domain.Repository;
using FluentValidation;
using Msg = Domain.ValidationErrorMessages;

namespace Domain.Validation
{
    public class CreateEntryValidator : BaseValidator<CreateEntryCommand>
    {
        public CreateEntryValidator(IUnitOfWork uow):base(uow)
        {
            SetPropertyRules();
            SetRelationRules();
        }

        private void SetPropertyRules()
        {
            RuleFor(e => e.DictionaryIndex).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
            RuleFor(e => e.WordID).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
        }

        private void SetRelationRules()
        {
            var repo = uow.Entries;
            var wordRepo = uow.Words;
            var dictRepo = uow.Dictionaries;

            RuleFor(m => m.WordID)
                .Must(wordRepo.ExistsByPrimaryKey)
                .WithMessage(m => Msg.EntityDoesNotExistByForeignKey<Entry, Word>(e => e.ID, m.WordID));
            
            RuleFor(m => m.DictionaryIndex)
                .Must(dictRepo.ExistsByPrimaryKey)
                .WithMessage(index => Msg.EntityDoesNotExistByForeignKey<Entry, Dictionary>(d => d.Index, index));

            RuleFor(m => new {m.WordID, m.DictionaryIndex})
                .Must(ids =>
                {
                    var existing = repo.GetOne(e => e.WordID == ids.WordID && e.DictionaryIndex == ids.DictionaryIndex);
                    return existing == null;
                }).WithMessage(Msg.DUPLICATE_ENTRY_DESC);

            RuleFor(m => new {m.WordID, m.DictionaryIndex})
                .Must(ids =>
                {
                    var word = wordRepo.GetByPrimaryKey(ids.WordID);
                    var dict = dictRepo.GetByPrimaryKey(ids.DictionaryIndex);
                    return
                        (word != null && dict != null) &&
                        String.Equals(word.SourceLanguageName, dict.LanguageInName, StringComparison.Ordinal);
                }).WithMessage(Msg.LANGUAGES_NOT_MATCH_DESC);
        }
    }
}
