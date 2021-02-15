using Domain.Models;
using Domain.Repository;
using Persistance.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Persistence.Tests.Repositories
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

        private void Disconnect()
        {
            changeContext();
            repo = new MeaningRepository(this.context);
            entryRepo = new EntryRepository(this.context);
            exampleRepo = new ExampleRepository(this.context);
        }

        private Dictionary DummyDict => new Dictionary
        {
            LanguageIn = new Language
            {
                Name = "dummy language"
            },
            LanguageOutName = "dummy language"
        };

        private Word DummyWord => new Word
        {
            SourceLanguageName = "dummy language",
            Value = "dummy value"
        };

        private Entry DummyEntry => new Entry
        {
            Dictionary = DummyDict,
            Word = DummyWord
        };

        private Meaning CreateMeaningWihtExamples()
        {
            var entity = new Meaning
            {
                Entry = DummyEntry,

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

        private Meaning CreateMeaningWithExamplesAndEntry()
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

                Entry = DummyEntry
            };

            return entity;
        }

        private Entry CreateEntryWithMeanings()
        {
            var entry = new Entry
            {
                Dictionary = DummyDict,
                Word = DummyWord,

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
            var entity = CreateMeaningWihtExamples();
            repo.Create(entity);

            var inDB = repo.All().FirstOrDefault();
            context.Entry(inDB).Collection(m => m.Examples).Load();

            Assert.NotNull(inDB);
            Assert.NotEmpty(exampleRepo.All());
            Assert.NotEmpty(inDB.Examples);
        }

        [Fact]
        public void Update_EntryIdCannotBeUpdated()
        {
            var entity = CreateMeaningWihtExamples();
            context.Meanings.Add(entity);
            context.SaveChanges();
            Disconnect();

            var entryId = entity.EntryID;

            var @new = new Meaning
            {
                ID = entity.ID,
                EntryID = 123,
                Value = "newValue",
                Notes = null,
                Examples = new List<Example>
                {
                    new()
                    {
                        Text = "text1",
                        Translation = "translation1"
                    },
                }
            };

            repo.Update(@new);
            Disconnect();

            var found = repo.GetByID(entity.ID);

            Assert.Equal(@new.Value, found.Value);
            Assert.Equal(@new.Notes, found.Notes);
            Assert.Single(found.Examples);
            Assert.NotEqual(found.EntryID, @new.EntryID);
            Assert.Equal(found.EntryID, entryId);

        }

        [Fact]
        public void GetByID_ReturnsEntityWithExamples()
        {
            var entity = CreateMeaningWihtExamples();
            context.Meanings.Add(entity);
            context.SaveChanges();
            Disconnect();

            var id = entity.ID;
            var found = repo.GetByID(id);

            Assert.NotNull(found);
            Assert.Equal(2, found.Examples.Count);
        }

        [Fact]
        public void GetByID_ReturnsEntityWithExamplesAndEntry()
        {
            var entity = CreateMeaningWithExamplesAndEntry();
            context.Meanings.Add(entity);
            context.SaveChanges();
            Disconnect();

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
            var entry = CreateEntryWithMeanings();
            context.Entries.Add(entry);
            context.SaveChanges();
            Disconnect();

            var entryID = entry.ID;

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
            var entry = CreateEntryWithMeanings();
            context.Entries.Add(entry);
            context.SaveChanges();
            Disconnect();

            var found = repo.GetByValueSubstring(query);
            Assert.NotNull(found);
            Assert.Empty(found);
        }

        [Fact]
        public void GetByNotesSubstring_ReturnsWithExamplesAndEntry()
        {
            var entry = CreateEntryWithMeanings();
            context.Entries.Add(entry);
            context.SaveChanges();
            Disconnect();

            var entryID = entry.ID;

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
            var entry = CreateEntryWithMeanings();
            context.Entries.Add(entry);
            context.SaveChanges();
            Disconnect();

            var found = repo.GetByNotesSubstring(query);
            Assert.NotNull(found);
            Assert.Empty(found);
        }

        [Fact]
        public void Delete_CascadeDeleteWorks()
        {
            var entity = CreateMeaningWihtExamples();
            repo.Create(entity);
            Disconnect();

            var id = entity.ID;

            var inDB = repo.GetByID(id);
            repo.Delete(inDB);

            Assert.Empty(repo.All());
            Assert.Empty(exampleRepo.All());
        }
    }
}
