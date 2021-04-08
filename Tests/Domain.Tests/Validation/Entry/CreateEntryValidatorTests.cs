using Domain.Dto;
using Domain.Models;
using Domain.Repository;
using Domain.Validation;
using FluentValidation;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace Domain.Tests.Validation
{
    public class CreateEntryValidatorTests : UowTestBase
    {
        AbstractValidator<CreateEntryCommand> sut;
        Mock<IWordRepository> _wordRepo = new Mock<IWordRepository>();
        Mock<IEntryRepository> _repo = new Mock<IEntryRepository>();
        Mock<IDictionaryRepository> _dictRepo = new Mock<IDictionaryRepository>();

        public CreateEntryValidatorTests()
        {
            wordRepo = _wordRepo;
            entryRepo = _repo;
            dictRepo = _dictRepo;
            sut = new CreateEntryValidator(this.uow.Object);
        }

        private void WordExists(bool result = true)
        {
            _wordRepo.Setup(_ => _.ExistsByPrimaryKey(It.IsAny<int>())).Returns(result);
        }

        private void DuplicateIs(Entry entity = null)
        {
            _repo.Setup(_ => _.GetOne(It.IsAny<System.Linq.Expressions.Expression<Func<Entry, bool>>>()))
                .Returns(entity);
        }

        private void DictionaryExists(bool result = true)
        {
            _dictRepo.Setup(_ => _.ExistsByPrimaryKey(It.IsAny<int>()))
                .Returns(result);
        }

        private void WordIs(Word entity)
        {
            _wordRepo.Setup(_ => _.GetByPrimaryKey(It.IsAny<int>())).Returns(entity);
        }

        private void DictionaryIs(Dictionary entity)
        {
            _dictRepo.Setup(_ => _.GetByPrimaryKey(It.IsAny<int>())).Returns(entity);
        }

        [Fact]
        public void EverythingIsGood_NoError()
        {
            var word = new Word
            {
                ID = 12,
                SourceLanguageName = "polish"
            };

            var dict = new Dictionary
            {
                Index = 1,
                LanguageInName = "polish",
                LanguageOutName = "english"
            };

            var command = new CreateEntryCommand
            {
                DictionaryIndex = dict.Index,
                WordID = word.ID,
            };

            WordExists();
            DictionaryExists();
            DuplicateIs(null);
            WordIs(word);
            DictionaryIs(dict);

            var result = sut.Validate(command);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void WordDoesNotExist_ReturnsError()
        {
            var dict = new Dictionary
            {
                Index = 1,
                LanguageInName = "german",
                LanguageOutName = "english"
            };

            var dto = new CreateEntryCommand
            {
                DictionaryIndex = dict.Index,
                WordID = 12,
            };

            WordExists(false);
            DuplicateIs();
            DictionaryExists();
            WordIs(null);
            DictionaryIs(dict);

            var result = sut.Validate(dto);

            Assert.False(result.IsValid);
            //Assert.Single(result.Errors); //TODO validator stop at first failure
            //Assert.Equal(
            //    ValidationErrorMessages.EntityDoesNotExistByForeignKey<Entry, Word>(e => e.ID, dto.WordID),
            //    result.Errors.First().ErrorMessage
            //);
        }

        [Fact]
        public void DuplicateExists_ReturnsError()
        {
            var word = new Word
            {
                ID = 12,
                SourceLanguageName = "polish"
            };

            var dict = new Dictionary
            {
                Index = 1,
                LanguageInName = "polish",
                LanguageOutName = "english"
            };

            var dto = new CreateEntryCommand
            {
                DictionaryIndex = dict.Index,
                WordID = word.ID,
            };

            var duplicate = new Entry
            {
                ID = 12,
                DictionaryIndex = dict.Index,
                WordID = word.ID,
            };

            WordExists();
            DuplicateIs(duplicate);
            DictionaryExists();
            WordIs(word);
            DictionaryIs(dict);

            var result = sut.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(
                ValidationErrorMessages.DUPLICATE_ENTRY_DESC,
                result.Errors.First().ErrorMessage
            );
        }

        [Fact]
        public void DictionaryDoesNotExist_ReturnsError()
        {
            var word = new Word
            {
                ID = 12,
                SourceLanguageName = "polish"
            };

            var dto = new CreateEntryCommand
            {
                DictionaryIndex = 34,
                WordID = word.ID,
            };

            WordExists();
            DuplicateIs();
            DictionaryExists(false);
            DictionaryIs(null);
            WordIs(word);

            var result = sut.Validate(dto);

            Assert.False(result.IsValid);
            //Assert.Single(result.Errors); TODO validator stop at first failure
            //Assert.Equal(
            //    ValidationErrorMessages.EntityDoesNotExistByForeignKey<Entry, Dictionary>(d => d.Index, dto.DictionaryIndex),
            //    result.Errors.First().ErrorMessage
            //);
        }

        [Fact]
        public void LangaugeCaseDoesNotMatch_ReturnsError()
        {
            var word = new Word
            {
                ID = 12,
                SourceLanguageName = "polish"
            };

            var dict = new Dictionary
            {
                Index = 1,
                LanguageInName = "Polish",
                LanguageOutName = "english"
            };

            var dto = new CreateEntryCommand
            {
                DictionaryIndex = dict.Index,
                WordID = word.ID,
            };

            WordExists();
            DuplicateIs();
            DictionaryExists();
            WordIs(word);
            DictionaryIs(dict);

            var result = sut.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(
                ValidationErrorMessages.LANGUAGES_NOT_MATCH_DESC,
                result.Errors.First().ErrorMessage
            );
        }

        [Fact]
        public void WordsLanguageIsDifferentThanDictionarysOne_ReturnsError()
        {
            var word = new Word
            {
                ID = 12,
                SourceLanguageName = "polish"
            };

            var dict = new Dictionary
            {
                Index = 1,
                LanguageInName = "german",
                LanguageOutName = "english"
            };

            var dto = new CreateEntryCommand
            {
                DictionaryIndex = dict.Index,
                WordID = word.ID,
            };

            WordExists();
            DuplicateIs();
            DictionaryExists();
            WordIs(word);
            DictionaryIs(dict);

            var result = sut.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(
                ValidationErrorMessages.LANGUAGES_NOT_MATCH_DESC,
                result.Errors.First().ErrorMessage
            );
        }
    }
}
