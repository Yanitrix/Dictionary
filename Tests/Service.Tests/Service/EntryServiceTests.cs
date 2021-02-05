using Data.Models;
using Moq;
using Data.Repository;
using System;
using System.Linq;
using Xunit;

namespace Service.Tests.Service
{
    public class EntryServiceTests : UowTestBase
    {
        IService<Entry> service;
        Mock<IWordRepository> _wordRepo = new Mock<IWordRepository>();
        Mock<IEntryRepository> _repo = new Mock<IEntryRepository>();
        Mock<IDictionaryRepository> _dictRepo = new Mock<IDictionaryRepository>();

        public EntryServiceTests()
        {
            wordRepo = _wordRepo;
            entryRepo = _repo;
            dictRepo = _dictRepo;
            service = new EntryService(this.uow.Object);
        }

        //helper methods for the mocks

        private void Exists(bool result = true)
        {
            _repo.Setup(_ => _.ExistsByID(It.IsAny<int>())).Returns(result);
        }

        private void HasMeanings(bool result)
        {
            _repo.Setup(_ => _.HasMeanings(It.IsAny<int>())).Returns(result);
        }

        private void WordExists(bool result = true)
        {
            _wordRepo.Setup(_ => _.ExistsByID(It.IsAny<int>())).Returns(result);
        }

        private void DuplicateIs(Entry entity = null)
        {
            _repo.Setup(_ => _.GetOne(It.IsAny<System.Linq.Expressions.Expression<Func<Entry, bool>>>()))
                .Returns(entity);
        }

        private void DictionaryExists(bool result = true)
        {
            _dictRepo.Setup(_ => _.ExistsByIndex(It.IsAny<int>()))
                .Returns(result);
        }

        private void WordIs(Word entity)
        {
            _wordRepo.Setup(_ => _.GetByID(It.IsAny<int>())).Returns(entity);
        }

        private void DictionaryIs(Dictionary entity)
        {
            _dictRepo.Setup(_ => _.GetByIndex(It.IsAny<int>())).Returns(entity);
        }

        private void ShouldUpdate()
        {
            _repo.Verify(_ => _.Update(It.IsAny<Entry>()), Times.Once);
        }

        private void ShouldNotUpdate()
        {
            _repo.Verify(_ => _.Update(It.IsAny<Entry>()), Times.Never);
        }

        private void ShouldAdd()
        {
            _repo.Verify(_ => _.Create(It.IsAny<Entry>()), Times.Once);
        }

        private void ShouldNotAdd()
        {
            _repo.Verify(_ => _.Create(It.IsAny<Entry>()), Times.Never);
        }

        [Fact]
        public void TryAdd_EverythingGood_AddsProperly()
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

            var entity = new Entry
            {
                DictionaryIndex = dict.Index,
                Dictionary = dict,
                WordID = word.ID,
                Word = word
            };

            WordExists();
            DictionaryExists();
            DuplicateIs(null);
            WordIs(word);
            DictionaryIs(dict);

            var result = service.Add(entity);

            ShouldAdd();
            Assert.Empty(result);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void TryAdd_WordDoesNotExist_ReturnsError()
        {

            var dict = new Dictionary
            {
                Index = 1,
                LanguageInName = "german",
                LanguageOutName = "english"
            };

            var entity = new Entry
            {
                DictionaryIndex = dict.Index,
                WordID = 12,
            };

            WordExists(false);
            DuplicateIs();
            DictionaryExists();
            WordIs(null);
            DictionaryIs(dict);

            var result = service.Add(entity);

            ShouldNotAdd();
            Assert.Single(result);
            Assert.Equal("Word does not exist.", result.First().Name);
        }

        [Fact]
        public void TryAdd_DuplicateExists_ReturnsError()
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

            var entity = new Entry
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

            var result = service.Add(entity);

            ShouldNotAdd();
            Assert.Single(result);
            Assert.Equal("Duplicate", result.First().Name);
        }

        [Fact]
        public void TryAdd_DictionaryDoesNotExist_ReturnsError()
        {
            var word = new Word
            {
                ID = 12,
                SourceLanguageName = "polish"
            };

            var entity = new Entry
            {
                DictionaryIndex = 34,
                WordID = word.ID,
            };

            WordExists();
            DuplicateIs();
            DictionaryExists(false);
            DictionaryIs(null);
            WordIs(word);

            var result = service.Add(entity);

            ShouldNotAdd();
            Assert.Single(result);
            Assert.Equal("Dictionary does not exist.", result.First().Name);
        }

        [Fact]
        public void TryAdd_LangaugeCaseDoesNotMatch_ReturnsError()
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

            var entity = new Entry
            {
                DictionaryIndex = dict.Index,
                Dictionary = dict,
                WordID = word.ID,
                Word = word
            };

            WordExists();
            DuplicateIs();
            DictionaryExists();
            WordIs(word);
            DictionaryIs(dict);

            var result = service.Add(entity);

            Assert.NotEmpty(result);
            Assert.False(result.IsValid);
            ShouldNotAdd();
        }

        [Fact]
        public void TryAdd_WordsLanguageIsDifferentThanDictionarysOne_ReturnsError()
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

            var entity = new Entry
            {
                DictionaryIndex = dict.Index,
                WordID = word.ID,
            };

            WordExists();
            DuplicateIs();
            DictionaryExists();
            WordIs(word);
            DictionaryIs(dict);

            var result = service.Add(entity);

            ShouldNotAdd();
            Assert.Single(result);
            Assert.Equal("Language does not match", result.First().Name);
        }
        [Fact]
        public void TryUpdate_EverythingsGood_UpdatesProperly()
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

            var entity = new Entry
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

            var result = service.Update(entity);

            Assert.Empty(result);
            Assert.True(result.IsValid);
            ShouldUpdate();
        }

        [Fact]
        public void TryUpdate_EntryDoesNotExist_ReturnsError()
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

            var entity = new Entry
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

            var result = service.Update(entity);

            ShouldNotUpdate();
            Assert.Single(result);
            Assert.False(result.IsValid);
            Assert.Equal("Entity does not exist.", result.First().Name);
        }

        [Fact]
        public void TryUpdate_EntryHasMeanings_ReturnsError()
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

            var entity = new Entry
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

            var result = service.Update(entity);

            ShouldNotUpdate();
            Assert.Single(result);
            Assert.False(result.IsValid);
            Assert.Equal("Entity cannot be updated", result.First().Name);
        }
        [Fact]
        public void TryUpdate_DuplicateExists_ReturnsError()
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

            var entity = new Entry
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
            var result = service.Update(entity);

            //assert
            ShouldNotUpdate();
            Assert.Single(result);
            Assert.False(result.IsValid);
            Assert.Equal("Duplicate", result.First().Name);
        }

        [Fact]
        public void TryUpdate_DuplicateExists_ButItsTheSameEntry_UpdatesProperly()
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

            var entity = new Entry
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
            var result = service.Update(entity);

            //assert
            ShouldUpdate();
            Assert.Empty(result);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void TryUpdate_DictionaryDoesNotExist_ReturnsError()
        {
            var word = new Word
            {
                ID = 12,
                SourceLanguageName = "polish"
            };

            var entity = new Entry
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

            var result = service.Update(entity);

            ShouldNotUpdate();
            Assert.Single(result);
            Assert.False(result.IsValid);
            Assert.Equal($"Dictionary with given Index: {entity.DictionaryIndex} was not found in the database. Create it before posting a(n) Entry", result.First().Description);
        }
    }
}
