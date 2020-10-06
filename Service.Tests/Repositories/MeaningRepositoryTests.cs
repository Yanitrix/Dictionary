using Data.Models;
using Data.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Service.Repository;

namespace Service.Tests.Repositories
{
    public class MeaningRepositoryTests : DbContextTestBase
    {
        public IMeaningRepository repo;
        public IEntryRepository entryRepo;
        public IExpressionRepository expressionRepo;

        public MeaningRepositoryTests()
        {
            repo = new MeaningRepository(this.context);
            entryRepo = new EntryRepository(this.context);
            expressionRepo = new ExpressionRepository(this.context);
        }

        private Meaning createEntityWihtExamples()
        {
            var entity = new Meaning
            {
                Value = "stick",
                Examples = new List<Expression>
                {
                    new Expression
                    {
                        Text = "gegessen sein",
                        Translation = "to be dead and buried"
                    },

                    new Expression
                    {
                        Text = "außer Betrieb sein",
                        Translation = "to be out or order"
                    }
                }
            };

            return entity;
        }

        private Meaning createEntityWithExamplesAndEntry()
        {
            var entity = new Meaning
            {
                Value = "stick",
                Examples = new List<Expression>
                {
                    new Expression
                    {
                        Text = "gegessen sein",
                        Translation = "to be dead and buried"
                    },

                    new Expression
                    {
                        Text = "außer Betrieb sein",
                        Translation = "to be out or order"
                    }
                },
                Entry = new Entry
                {
                    DictionaryIndex = 1,
                    WordID = 1,
                }
            };

            return entity;
        }

        private Entry createEntryWithMeanings()
        {
            var entry = new Entry
            {
                DictionaryIndex = 1,
                WordID = 1,
                Meanings = new HashSet<Meaning>
                {
                    new Meaning
                    {
                        Notes = "substring",
                        Value = "stringsub",
                        Examples = new List<Expression>
                        {
                            new Expression{}
                        }
                    },

                    new Meaning
                    {
                        Notes = "this is a substring",
                        Value = "pan",
                        Examples = new List<Expression>
                        {
                            new Expression{},
                            new Expression{}
                        }
                    },

                    new Meaning
                    {
                        Notes = "hstus",
                        Value = "this is not a stringsub",
                        Examples = new List<Expression>
                        {
                            new Expression{},
                            new Expression{},
                            new Expression{}
                        }
                    },

                },

            };

            return entry;
        }

        [Fact]
        public void AddWithChildren_ChildrenExist()
        {
            var entity = createEntityWihtExamples();
            repo.Create(entity);
            repo.Detach(entity);

            var inDB = repo.All().FirstOrDefault();
            context.Entry(inDB).Collection(m => m.Examples).Load();

            Assert.NotNull(inDB);
            Assert.NotEmpty(expressionRepo.All());
            Assert.NotEmpty(inDB.Examples);

        }

        [Fact]
        public void GetByID_ReturnsEntityWithExamples()
        {
            var entity = createEntityWihtExamples();
            repo.Create(entity);
            repo.Detach(entity);

            var id = entity.ID;
            var found = repo.GetByID(id);

            Assert.NotNull(found);
            Assert.Equal(2, found.Examples.Count);
        }

        [Fact]
        public void GetByID_ReturnsEntityWithExamplesAndEntry()
        {
            var entity = createEntityWithExamplesAndEntry();
            repo.Create(entity);
            repo.Detach(entity);

            var id = entity.ID;

            var found = repo.GetByID(id);

            Assert.NotNull(found);
            Assert.NotEmpty(found.Examples);
            Assert.NotNull(found.Entry);
            Assert.Equal(1, found.Entry.DictionaryIndex);
            Assert.Equal(1, found.Entry.WordID);
        }

        //test cases: empty or null
        [Fact]
        public void GetByValueSubstring_ReturnsWithExamplesAndEntry()
        {
            var entity = createEntryWithMeanings();
            entryRepo.Create(entity);
            entryRepo.Detach(entity);

            var entryID = entity.ID;

            var found = repo.GetByValueSubstring("stringsub");
            var indexed = new List<Meaning>(found);

            Assert.Equal(2, indexed.Count);

            Assert.NotNull(indexed[0].Entry);
            Assert.Equal(entryID, indexed[0].Entry.ID);
            Assert.Equal(1, indexed[0].Examples.Count);

            Assert.NotNull(indexed[1].Entry);
            Assert.Equal(entryID, indexed[1].Entry.ID);
            Assert.Equal(3, indexed[1].Examples.Count);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\r")]
        [InlineData("\r\t")]
        [InlineData(" ")]
        [InlineData("")]
        public void GetByValueSubstring_NullOrEmpty_ReturnsEmptyCollection(String query)
        {
            var entity = createEntryWithMeanings();
            entryRepo.Create(entity);
            entryRepo.Detach(entity);

            var found = repo.GetByValueSubstring(query);
            Assert.NotNull(found);
            Assert.Empty(found);
        }

        [Fact]
        public void GetByNotesSubstring_ReturnsWithExamplesAndEntry()
        {
            var entity = createEntryWithMeanings();
            entryRepo.Create(entity);
            entryRepo.Detach(entity);

            var entryID = entity.ID;

            var found = repo.GetByNotesSubstring("substring");
            var indexed = new List<Meaning>(found);

            Assert.Equal(2, indexed.Count);

            Assert.NotNull(indexed[0].Entry);
            Assert.Equal(entryID, indexed[0].Entry.ID);
            Assert.Equal(1, indexed[0].Examples.Count);

            Assert.NotNull(indexed[1].Entry);
            Assert.Equal(entryID, indexed[1].Entry.ID);
            Assert.Equal(2, indexed[1].Examples.Count);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\r")]
        [InlineData("\r\t")]
        [InlineData(" ")]
        [InlineData("")]
        public void GetByNotesSubstring_NullOrEmpty_ReturnsEmptyCollection(String query)
        {
            var entity = createEntryWithMeanings();
            entryRepo.Create(entity);
            entryRepo.Detach(entity);

            var found = repo.GetByNotesSubstring(query);
            Assert.NotNull(found);
            Assert.Empty(found);
        }

        [Fact]
        public void Delete_CascadeDeleteWorks()
        {
            var entity = createEntityWihtExamples();
            repo.Create(entity);
            repo.Detach(entity);

            var id = entity.ID;

            var inDB = repo.GetByID(id);
            repo.Delete(inDB);

            Assert.Empty(repo.All());
            Assert.Empty(expressionRepo.All());
        }
    }
}
