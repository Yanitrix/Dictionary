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
        public IExampleRepository exampleRepo;

        public MeaningRepositoryTests()
        {
            repo = new MeaningRepository(this.context);
            entryRepo = new EntryRepository(this.context);
            exampleRepo = new ExampleRepository(this.context);
        }

        private void reloadRepos()
        {
            changeContext();
            repo = new MeaningRepository(this.context);
            entryRepo = new EntryRepository(this.context);
            exampleRepo = new ExampleRepository(this.context);
        }

        private Dictionary dummyDic() => new Dictionary
        {
            LanguageIn = new Language
            {
                Name = "dummy language"
            },
            LanguageOutName = "dummy language"
        };

        private Word dummyWord() => new Word
        {
            SourceLanguageName = "dummy language",
            Value = "dummy value"
        };

        private Entry dummyEntry() => new Entry
        {
            Dictionary = dummyDic(),
            Word = dummyWord()
        };

        private Meaning createMeaningWihtExamples()
        {
            var entity = new Meaning
            {
                Entry = dummyEntry(),

                Value = "stick",
                Examples = new List<Example>
                {
                    new Example
                    {
                        Text = "gegessen sein",
                        Translation = "to be dead and buried"
                    },

                    new Example
                    {
                        Text = "außer Betrieb sein",
                        Translation = "to be out or order"
                    }
                }
            };

            return entity;
        }

        private Meaning createMeaningWithExamplesAndEntry()
        {
            var entity = new Meaning
            {
                Value = "stick",
                Examples = new List<Example>
                {
                    new Example
                    {
                        Text = "gegessen sein",
                        Translation = "to be dead and buried"
                    },

                    new Example
                    {
                        Text = "außer Betrieb sein",
                        Translation = "to be out or order"
                    }
                },

                Entry = dummyEntry()
            };

            return entity;
        }

        private Entry createEntryWithMeanings()
        {
            var entry = new Entry
            {
                Dictionary = dummyDic(),
                Word = dummyWord(),

                Meanings = new HashSet<Meaning>
                {
                    new Meaning
                    {
                        Notes = "substring",
                        Value = "stringsub",
                        Examples = new List<Example>
                        {
                            new Example
                            {
                                Text = "text",
                                Translation = "translation"
                            },
                        }
                    },

                    new Meaning
                    {
                        Notes = "this is a substring",
                        Value = "pan",
                        Examples = new List<Example>
                        {
                            new Example
                            {
                                Text = "text1",
                                Translation = "translation1"
                            },
                            new Example
                            {
                                Text = "text2",
                                Translation = "translation2"
                            },
                        }
                    },

                    new Meaning
                    {
                        Notes = "hstus",
                        Value = "this is not a stringsub",
                        Examples = new List<Example>
                        {
                            new Example
                            {
                                Text = "text3",
                                Translation = "translation3"
                            },
                            new Example
                            {
                                Text = "text4",
                                Translation = "translation4"
                            },
                            new Example
                            {
                                Text = "text5",
                                Translation = "translation5"
                            },
                        }
                    },
                },
            };

            return entry;
        }

        [Fact]
        public void AddWithChildren_ChildrenExist()
        {
            var entity = createMeaningWihtExamples();
            repo.Create(entity);

            var inDB = repo.All().FirstOrDefault();
            context.Entry(inDB).Collection(m => m.Examples).Load();

            Assert.NotNull(inDB);
            Assert.NotEmpty(exampleRepo.All());
            Assert.NotEmpty(inDB.Examples);
        }

        [Fact]
        public void GetByID_ReturnsEntityWithExamples()
        {
            var entity = createMeaningWihtExamples();
            repo.Create(entity);
            reloadRepos();

            var id = entity.ID;
            var found = repo.GetByID(id);

            Assert.NotNull(found);
            Assert.Equal(2, found.Examples.Count);
        }

        [Fact]
        public void GetByID_ReturnsEntityWithExamplesAndEntry()
        {
            var entity = createMeaningWithExamplesAndEntry();
            repo.Create(entity);
            reloadRepos();

            var id = entity.ID;

            var found = repo.GetByIDWithEntry(id);

            Assert.NotNull(found);
            Assert.NotEmpty(found.Examples);
            Assert.NotNull(found.Entry);
        }

        //test cases: empty or null
        [Fact]
        public void GetByValueSubstring_ReturnsWithExamplesAndEntry()
        {
            var entity = createEntryWithMeanings();
            entryRepo.Create(entity);
            reloadRepos();

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
            reloadRepos();

            var found = repo.GetByValueSubstring(query);
            Assert.NotNull(found);
            Assert.Empty(found);
        }

        [Fact]
        public void GetByNotesSubstring_ReturnsWithExamplesAndEntry()
        {
            var entity = createEntryWithMeanings();
            entryRepo.Create(entity);
            reloadRepos();

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
            reloadRepos();

            var found = repo.GetByNotesSubstring(query);
            Assert.NotNull(found);
            Assert.Empty(found);
        }

        [Fact]
        public void Delete_CascadeDeleteWorks()
        {
            var entity = createMeaningWihtExamples();
            repo.Create(entity);
            reloadRepos();

            var id = entity.ID;

            var inDB = repo.GetByID(id);
            repo.Delete(inDB);

            Assert.Empty(repo.All());
            Assert.Empty(exampleRepo.All());
        }
    }
}
