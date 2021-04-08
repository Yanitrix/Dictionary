using Domain.Dto;
using Domain.Mapper;
using Domain.Models;
using Domain.Queries;
using Domain.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Tests.Handlers.Queries
{
    public class DictionaryContainingLanguagesQueryHandlerTests
    {
        private Mock<IDictionaryRepository> repo = new();
        private Mock<IMapper> mapper = new();
        private IQueryHandler<DictionaryContainingLanguagesQuery, IEnumerable<GetDictionary>> sut;

        public DictionaryContainingLanguagesQueryHandlerTests()
        {
            mapper
                .Setup(_ => _.Map<Dictionary, GetDictionary>(It.IsAny<Dictionary>()))
                .Returns<Dictionary>(d => new GetDictionary
                {
                    Index = d.Index,
                    LanguageIn = d.LanguageInName,
                    LanguageOut = d.LanguageOutName
                });

            sut = new DictionaryContainingLanguagesQueryHandler(repo.Object, mapper.Object);

            //preapre fake data
            // polish -> english
            // english -> polish
            // polish -> russian
            // english -> german
            // german -> japanese
            var polishToEnglish = new Dictionary
            {
                Index = 1,
                LanguageInName = "polish",
                LanguageOutName = "english"
            };
            
            var englishToPolish = new Dictionary
            {
                Index = 2,
                LanguageInName = "english",
                LanguageOutName = "polish"
            };
            
            var polishToRussian = new Dictionary
            {
                Index = 3,
                LanguageInName = "polish",
                LanguageOutName = "russian"
            };
            
            var englishToGerman = new Dictionary
            {
                Index = 5,
                LanguageInName = "english",
                LanguageOutName = "german"
            };
            
            var germanToJapanese = new Dictionary
            {
                Index = 5,
                LanguageInName = "german",
                LanguageOutName = "japanese"
            };

            var dictionaries = new[] { polishToEnglish, englishToPolish, polishToRussian, englishToGerman, germanToJapanese };

            //mock return values

            repo.Setup(_ => _.GetAllByLanguage(It.IsAny<String>())).Returns<String>(
                s => dictionaries.Where(d => d.LanguageInName == s || d.LanguageOutName == s));

            repo.Setup(_ => _.GetAllByLanguageIn(It.IsAny<String>())).Returns<String>(
                s => dictionaries.Where(d => d.LanguageInName == s));

            repo.Setup(_ => _.GetAllByLanguageOut(It.IsAny<String>())).Returns<String>(
                s => dictionaries.Where(d => d.LanguageOutName == s));

            repo.Setup(_ => _.GetByLanguageInAndOut(It.IsAny<String>(), It.IsAny<String>()))
                .Returns<String, String>(
                    (langin, langout) => dictionaries.FirstOrDefault(
                        d => d.LanguageInName == langin && d.LanguageOutName == langout)
                );

            repo.Setup(_ => _.All()).Returns(dictionaries);
            
        }

        [Fact]
        public void LangaugePresent_LanguagInAndLanguageOutAreNull_CallsGetAllByLanguage()
        {
            var query = new DictionaryContainingLanguagesQuery
            {
                Language = "polish",
                LanguageIn = null,
                LanguageOut = null
            };

            //should be polish - english{1}; english - polish{2}; polish - russian{3}
            var found = sut.Handle(query);

            //assert
            repo.Verify(_ => _.GetAllByLanguage(query.Language), Times.Once);
            Assert.NotEmpty(found);
            Assert.Equal(3, found.Count());
            Assert.Equal(new int[] { 1, 2, 3 }, found.Select(d => d.Index));
        }

        [Fact]
        public void LanguageInPresent_LanguageAndLanguageOutAreNull_CallsGetAllByLanguageIn()
        {
            var query = new DictionaryContainingLanguagesQuery
            {
                Language = null,
                LanguageIn = "polish",
                LanguageOut = null
            };

            //polish - english, polish - russian
            var found = sut.Handle(query);

            //assert
            repo.Verify(_ => _.GetAllByLanguageIn(query.LanguageIn), Times.Once);
            Assert.NotEmpty(found);
            Assert.Equal(2, found.Count());
            Assert.Equal(new int[] { 1, 3 }, found.Select(x => x.Index));
        }

        [Fact]
        public void LanguageOutPresent_LanguageAndLanguageInAreNull_CallsGetAllByLanguageOut()
        {
            var query = new DictionaryContainingLanguagesQuery
            {
                Language = null,
                LanguageIn = null,
                LanguageOut = "polish"
            };

            //english - polish only
            var found = sut.Handle(query);

            //assert
            repo.Verify(_ => _.GetAllByLanguageOut(query.LanguageOut), Times.Once);
            Assert.NotEmpty(found);
            Assert.Single(found);
            Assert.Equal(new int[] { 2 }, found.Select(x => x.Index));
        }

        [Fact]
        public void LanguageInAndLanguageOutArePresent_LanguageIsNull_CallsGetByLanguageInAndOut()
        {
            var query = new DictionaryContainingLanguagesQuery
            {
                Language = null,
                LanguageIn = "polish",
                LanguageOut = "english"
            };

            //polish - english
            var found = sut.Handle(query);

            //assert
            repo.Verify(_ => _.GetByLanguageInAndOut(query.LanguageIn, query.LanguageOut), Times.Once);
            Assert.NotEmpty(found);
            Assert.Single(found);
            Assert.Equal(new int[] { 1 }, found.Select(x => x.Index));
        }

        [Fact]
        public void LanguageInAndOutPresent_LanguageIsNull_CallsGetByLanguageInAndOut_MethodReturnsNull_ReturnsEmpty()
        {
            var query = new DictionaryContainingLanguagesQuery
            {
                Language = null,
                LanguageIn = "polish",
                LanguageOut = "english"
            };

            //explicit mock
            repo
                .Setup(_ => _.GetByLanguageInAndOut(query.LanguageIn, query.LanguageOut))
                .Returns((Dictionary)null); 
            //polish - english
            var found = sut.Handle(query);

            //assert
            repo.Verify(_ => _.GetByLanguageInAndOut(query.LanguageIn, query.LanguageOut), Times.Once);
            Assert.NotNull(found);
            Assert.Empty(found);
        }

        [Fact]
        public void AllParametersNull_CallsAll()
        {
            var query = new DictionaryContainingLanguagesQuery
            {
                Language = null,
                LanguageIn = null,
                LanguageOut = null
            };

            //all
            var found = sut.Handle(query);

            //assert
            repo.Verify(_ => _.All(), Times.Once);
            Assert.NotEmpty(found);
            Assert.Equal(5, found.Count());
        }

        [Fact]
        public void AllParametersPresent_ReturnsEmpty()
        {
            var query = new DictionaryContainingLanguagesQuery
            {
                Language = "polish",
                LanguageIn = "polish",
                LanguageOut = "english"
            };

            //none
            var found = sut.Handle(query);

            //assert
            Assert.NotNull(found);
            Assert.Empty(found);
        }

        [Fact]
        public void LanguageInNull_LanguageAndLanguageOutPresent_ReturnsEmpty()
        {
            var query = new DictionaryContainingLanguagesQuery
            {
                Language = "polish",
                LanguageIn = null,
                LanguageOut = "english"
            };

            //none
            var found = sut.Handle(query);

            //assert
            Assert.NotNull(found);
            Assert.Empty(found);
        }

        [Fact]
        public void LanguageOutNull_LanguageAndLanguageInPresent_ReturnsEmpty()
        {
            var query = new DictionaryContainingLanguagesQuery
            {
                Language = "polish",
                LanguageIn = "english",
                LanguageOut = null
            };

            //none
            var found = sut.Handle(query);

            //assert
            Assert.NotNull(found);
            Assert.Empty(found);
        }
    }
}
