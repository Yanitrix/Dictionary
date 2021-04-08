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
    public class UpdateEntryValidatorTests : UowTestBase
    {
        AbstractValidator<UpdateEntryCommand> sut;
        Mock<IWordRepository> _wordRepo = new Mock<IWordRepository>();
        Mock<IEntryRepository> _repo = new Mock<IEntryRepository>();
        Mock<IDictionaryRepository> _dictRepo = new Mock<IDictionaryRepository>();

        public UpdateEntryValidatorTests()
        {
            wordRepo = _wordRepo;
            entryRepo = _repo;
            dictRepo = _dictRepo;
            sut = new UpdateEntryValidator(this.uow.Object);
        }

        private void Exists(bool result = true)
        {
            _repo.Setup(_ => _.ExistsByPrimaryKey(It.IsAny<int>())).Returns(result);
        }

        private void HasMeanings(bool result)
        {
            _repo.Setup(_ => _.HasMeanings(It.IsAny<int>())).Returns(result);
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
        public void EverythingsGood_UpdatesProperly()
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

            var dto = new UpdateEntryCommand
            {
                ID = 928,
                DictionaryIndex = dict.Index,
                WordID = word.ID,
            };

            Exists();
            HasMeanings(false);

            WordExists();
            DuplicateIs();
            DictionaryExists();
            WordIs(word);
            DictionaryIs(dict);

            var result = sut.Validate(dto);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void EntryDoesNotExist_ReturnsError()
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

            var dto = new UpdateEntryCommand
            {
                ID = 928,
                DictionaryIndex = dict.Index,
                WordID = word.ID,
            };

            Exists(false);
            HasMeanings(false);

            WordExists();
            DuplicateIs(null);
            DictionaryExists();
            WordIs(word);
            DictionaryIs(dict);

            var result = sut.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(
                ValidationErrorMessages.ThereIsNothingToUpdate<Entry>(),
                result.Errors.First().ErrorMessage
            );
        }

        [Fact]
        public void EntryHasMeanings_ReturnsError()
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

            var dto = new UpdateEntryCommand
            {
                ID = 928,
                DictionaryIndex = dict.Index,
                WordID = word.ID,
            };

            Exists();
            HasMeanings(true);

            WordExists();
            DuplicateIs(null);
            DictionaryExists();
            WordIs(word);
            DictionaryIs(dict);

            var result = sut.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(
                ValidationErrorMessages.CANNOT_UPDATE_ENTRY_DESC,
                result.Errors.First().ErrorMessage
            );
        }

        [Fact]
        public void DuplicateExists_ReturnsError()
        {
            var word = new Word
            {
                ID = 1,
                SourceLanguageName = "german"
            };

            var dict = new Dictionary
            {
                Index = 2,
                LanguageInName = "german",
                LanguageOutName = "english"
            };

            var dto = new UpdateEntryCommand
            {
                ID = 3,
                DictionaryIndex = dict.Index,
                WordID = word.ID,
            };

            var duplicate = new Entry
            {
                ID = 4,
                DictionaryIndex = dict.Index,
                WordID = word.ID
            };

            Exists();
            HasMeanings(false);

            WordExists();
            DuplicateIs(duplicate);
            DictionaryExists();
            WordIs(word);
            DictionaryIs(dict);


            //act
            var result = sut.Validate(dto);

            //assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(
                ValidationErrorMessages.DUPLICATE_ENTRY_DESC,
                result.Errors.First().ErrorMessage
            );
        }

        [Fact]
        public void DuplicateExists_ButItsTheSameEntry_UpdatesProperly()
        {
            var word = new Word
            {
                ID = 1,
                SourceLanguageName = "german"
            };

            var dict = new Dictionary
            {
                Index = 2,
                LanguageInName = "german",
                LanguageOutName = "english"
            };

            var dto = new UpdateEntryCommand
            {
                ID = 3,
                DictionaryIndex = dict.Index,
                WordID = word.ID,
            };
            //the same as ^
            var duplicate = new Entry
            {
                ID = 3,
                DictionaryIndex = dict.Index,
                WordID = word.ID
            };

            Exists();
            HasMeanings(false);

            WordExists();
            DuplicateIs(duplicate);
            DictionaryExists();
            WordIs(word);
            DictionaryIs(dict);

            //act
            var result = sut.Validate(dto);

            //TODO check id
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void DictionaryDoesNotExist_ReturnsError()
        {
            var word = new Word
            {
                ID = 12,
                SourceLanguageName = "polish"
            };

            var dto = new UpdateEntryCommand
            {
                ID = 928,
                DictionaryIndex = 16,
                WordID = word.ID,
            };

            Exists();
            HasMeanings(false);


            WordExists();
            DuplicateIs(null);
            DictionaryExists(false);
            WordIs(word);
            DictionaryIs(null);

            var result = sut.Validate(dto);

            Assert.False(result.IsValid);
            //Assert.Single(result.Errors); TODO validators
            //Assert.Equal(
            //    ValidationErrorMessages.EntityDoesNotExistByForeignKey<Entry, Dictionary>(d => d.Index, dto.DictionaryIndex),
            //    result.Errors.First().ErrorMessage
            //);
        }
    }
}
