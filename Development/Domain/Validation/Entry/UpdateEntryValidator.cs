using System;
using Domain.Dto;
using Domain.Messages;
using Domain.Models;
using Domain.Repository;
using FluentValidation;
using Msg = Domain.ValidationErrorMessages;

namespace Domain.Validation
{
    public class UpdateEntryValidator : BaseValidator<UpdateEntryCommand>
    {
        public UpdateEntryValidator(IUnitOfWork uow):base(uow)
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

            RuleFor(m => m.ID)
                .Must(id => repo.ExistsByPrimaryKey(id))
                .WithMessage(Msg.ThereIsNothingToUpdate<Entry>());

            //TODO DRY, export it to some base class because there's code duplication
            RuleFor(m => m)
                .Must(m => !repo.HasMeanings(m.ID))
                .WithMessage(Msg.CANNOT_UPDATE_ENTRY_DESC);
            
            RuleFor(m => m.WordID)
                .Must(id => wordRepo.ExistsByPrimaryKey(id))
                .WithMessage(m => Msg.EntityDoesNotExistByForeignKey<Entry, Word>(e => e.ID, m.WordID));
            
            RuleFor(m => m.DictionaryIndex)
                .Must(index => dictRepo.ExistsByPrimaryKey(index))
                .WithMessage(index => Msg.EntityDoesNotExistByForeignKey<Entry, Dictionary>(d => d.Index, index));

            RuleFor(m => m)
                .Must(m =>
                {
                    var existing = repo.GetOne(e => e.WordID == m.WordID && e.DictionaryIndex == m.DictionaryIndex);
                    if (existing == null)
                        return true;
                    if (existing != null && existing.ID == m.ID)
                        return true;
                    return false;
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
