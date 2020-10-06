using Data.Models;
using Moq;
using Service.Repository;
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
            service = new EntryService(this.uow.Object, VMoq.Instance<Entry>());
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

            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == dict.Index))).Returns(true);
            _wordRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == word.ID))).Returns(true);
            _dictRepo.Setup(_ => _.GetByIndex(It.Is<int>(i => i == dict.Index))).Returns(dict);
            _wordRepo.Setup(_ => _.GetByID(It.Is<int>(i => i == word.ID))).Returns(word);
            _repo.Setup(_ => _.ExistsByWord(It.IsAny<int>())).Returns(false);

            var result = service.TryAdd(entity);

            _repo.Verify(_ => _.Create(It.IsAny<Entry>()), Times.Once);
            Assert.Empty(result);
            Assert.True(result.IsValid);
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

            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == dict.Index))).Returns(true);
            _wordRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == word.ID))).Returns(true);
            _dictRepo.Setup(_ => _.GetByIndex(It.Is<int>(i => i == dict.Index))).Returns(dict);
            _wordRepo.Setup(_ => _.GetByID(It.Is<int>(i => i == word.ID))).Returns(word);
            _repo.Setup(_ => _.ExistsByWord(It.IsAny<int>())).Returns(false);

            var result = service.TryAdd(entity);

            _repo.Verify(_ => _.Create(It.IsAny<Entry>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Language does not match", result.First().Key);
        }

        [Fact]
        public void TryAdd_EntryWithGivenWordAlreadyExists_ReturnsError()
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

            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == dict.Index))).Returns(true);
            _wordRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == word.ID))).Returns(true);
            _dictRepo.Setup(_ => _.GetByIndex(It.Is<int>(i => i == dict.Index))).Returns(dict);
            _wordRepo.Setup(_ => _.GetByID(It.Is<int>(i => i == word.ID))).Returns(word);
            _repo.Setup(_ => _.ExistsByWord(It.Is<int>(i => i == word.ID))).Returns(true);

            var result = service.TryAdd(entity);

            _repo.Verify(_ => _.Create(It.IsAny<Entry>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Duplicate", result.First().Key);
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

            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == 34))).Returns(false);
            _wordRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == word.ID))).Returns(true);
            _wordRepo.Setup(_ => _.GetByID(It.Is<int>(i => i == word.ID))).Returns(word);
            _repo.Setup(_ => _.ExistsByWord(It.IsAny<int>())).Returns(false);

            var result = service.TryAdd(entity);

            _repo.Verify(_ => _.Create(It.IsAny<Entry>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Dictionary not found", result.First().Key);
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

            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == dict.Index))).Returns(true);
            _wordRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == 12))).Returns(false);
            _dictRepo.Setup(_ => _.GetByIndex(It.Is<int>(i => i == dict.Index))).Returns(dict);
            _wordRepo.Setup(_ => _.GetByID(It.Is<int>(i => i == 12))).Returns((Word)null);
            _repo.Setup(_ => _.ExistsByWord(It.Is<int>(i => i == 12))).Returns(false);

            var result = service.TryAdd(entity);

            _repo.Verify(_ => _.Create(It.IsAny<Entry>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Word not found", result.First().Key);
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

            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == dict.Index))).Returns(true);
            _wordRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == word.ID))).Returns(true);
            _dictRepo.Setup(_ => _.GetByIndex(It.Is<int>(i => i == dict.Index))).Returns(dict);
            _wordRepo.Setup(_ => _.GetByID(It.Is<int>(i => i == word.ID))).Returns(word);
            _repo.Setup(_ => _.ExistsByWord(It.Is<int>(i => i == word.ID))).Returns(false);
            _repo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.ID))).Returns(true);

            var result = service.TryUpdate(entity);

            Assert.Empty(result);
            Assert.True(result.IsValid);
            _repo.Verify(_ => _.Update(It.IsAny<Entry>()), Times.Once);
        }

        [Fact]
        public void TryUpdate_EntryNotFound_ReturnsError()
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
                ID = 928,
                DictionaryIndex = dict.Index,
                WordID = word.ID,
            };

            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == dict.Index))).Returns(true);
            _wordRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == word.ID))).Returns(true);
            _dictRepo.Setup(_ => _.GetByIndex(It.Is<int>(i => i == dict.Index))).Returns(dict);
            _wordRepo.Setup(_ => _.GetByID(It.Is<int>(i => i == word.ID))).Returns(word);
            _repo.Setup(_ => _.ExistsByWord(It.Is<int>(i => i == word.ID))).Returns(true);
            _repo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.ID))).Returns(false);

            var result = service.TryUpdate(entity);

            _repo.Verify(_ => _.Update(It.IsAny<Entry>()), Times.Never);
            Assert.Single(result);
            Assert.False(result.IsValid);
            Assert.Equal("Entity does not exist", result.First().Key);
        }

        [Fact]
        public void TryUpdate_EntryFoundButOtherError_ReturnsError()
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

            _dictRepo.Setup(_ => _.ExistsByIndex(It.Is<int>(i => i == entity.DictionaryIndex))).Returns(false);
            _wordRepo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == word.ID))).Returns(true);
            _wordRepo.Setup(_ => _.GetByID(It.Is<int>(i => i == word.ID))).Returns(word);
            _repo.Setup(_ => _.ExistsByWord(It.Is<int>(i => i == word.ID))).Returns(false);
            _repo.Setup(_ => _.ExistsByID(It.Is<int>(i => i == entity.ID))).Returns(true);

            var result = service.TryUpdate(entity);

            _repo.Verify(_ => _.Update(It.IsAny<Entry>()), Times.Never);
            Assert.Single(result);
            Assert.False(result.IsValid);
            Assert.Equal("Dictionary with given Index does not exist in the database. Please create it before posting an Entry", result.First().Value);
        }
    }
}
