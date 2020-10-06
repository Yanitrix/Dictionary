﻿using Api.Service;
using Data.Models;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace Service.Tests.Service
{
    public class LanguageServiceTests
    {
        IService<Language> service;
        Mock<ILanguageRepository> langRepoMock = new Mock<ILanguageRepository>();
        AbstractValidator<Language> validatorMock = new ValidatorMoq<Language>();


        public LanguageServiceTests()
        {
            service = new LanguageService(validatorMock, langRepoMock.Object);
        }

        [Fact]
        public void TryAdd_LanguageExists_ReturnsError()
        {
            const string name = "name";
            langRepoMock.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(true);

            var lang = new Language
            {
                Name = name
            };

            var result = service.TryAdd(lang);

            Assert.Single(result);
            Assert.Equal("Duplicate", result.First().Key);
        }

        [Fact]
        public void TryAdd_LanguageDoesNotExist_AddsProperly()
        {
            IList<Language> repo = new List<Language>();

            langRepoMock.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(false);
            langRepoMock.Setup(_ => _.Create(It.IsAny<Language>())).Callback<Language>(x => repo.Add(x));

            var lang = new Language();
            var result = service.TryAdd(lang);

            Assert.Empty(result);
            Assert.Single(repo);
        }
        
        [Fact]
        public void TryUpdate_Impossible_AlwaysReturnsError()
        {
            langRepoMock.Setup(_ => _.ExistsByName(It.IsAny<String>())).Returns(false);

            var lang = new Language();
            var result = service.TryUpdate(lang);

            Assert.Single(result);
            Assert.Equal("Entity cannot be updated", result.First().Key);
        }
    }
}