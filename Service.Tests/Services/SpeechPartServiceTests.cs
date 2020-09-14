using Api.Service;
using Api.Service.Validation;
using Data.Tests;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace Service.Tests.Services
{
    public class SpeechPartServiceTests : DbContextTestBase
    {


        private SpeechPartService service;

        private DataInitializer data = new DataInitializer();

        public SpeechPartServiceTests()
        {
            service = new SpeechPartService(this.context, new SpeechPartValidator());
            service.ValidationDictionary = new ValidationDictionary();
            putData();
        }

        private void putData()
        {
            context.Languages.AddRange(data.English, data.German, data.Polish);
            context.SaveChanges();
        }

        [Fact]
        public void Add_WithEmptyProperties_ShouldExist()
        {
            var entity = data.PolishSpeechParts[0];
            entity.Properties = Enumerable.Empty<SpeechPartProperty>();
            service.Create(entity);
            var entityInDb = service.GetByLanguageAndOwnName(entity.LanguageName, entity.Name);
            Assert.NotNull(entityInDb);
        }

        [Fact]
        public void AddRange_WithEmptyProperties_ShouldExistAndBeOwnedByLanguage()
        {
            var entities = data.GermanSpeechParts;
            service.CreateRange(entities);

            foreach(var en in entities) en.Properties = Enumerable.Empty<SpeechPartProperty>();

            var entitiesInDb = context.Languages.Include(l => l.SpeechParts).AsNoTracking().FirstOrDefault(l => l.Name == entities[0].LanguageName).SpeechParts;

            Assert.Equal(entities.Length, entitiesInDb.Count());
            Assert.Equal(entities, entitiesInDb);
        }
    }
}
