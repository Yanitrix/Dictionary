﻿using Data.Dto;
using Data.Mapper;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Data.Tests.Mapper
{
    public class CustomMapperTests
    {

        private readonly IMapper sut;

        public CustomMapperTests()
        {
            this.sut = IMapper.CreateMapper(new MappingConfig());
        }

        [Fact]
        public void CreateWordToWord_ValuesShouldBeEqualAndNoExceptionThrown()
        {
            CreateWord dto = new()
            {
                Value = "value",
                SourceLanguageName = "sourceLanguageName",
                Properties = new HashSet<WordPropertyDto>()
                {
                    new WordPropertyDto
                    {
                        Name = "gender",
                        Values = new List<String>{"masculine", "feminine"}
                    },

                    new WordPropertyDto
                    {
                        Name = "plural form",
                        Values = new List<String>{"values"}
                    }
                }
            };

            var domain = sut.Map<CreateWord, Word>(dto);
            var first = domain.Properties.First();
            var second = domain.Properties.Last();

            Assert.Equal("value", domain.Value);
            Assert.Equal("sourceLanguageName", domain.SourceLanguageName);
            Assert.Equal(2, domain.Properties.Count);
            Assert.Equal("gender", first.Name);
            Assert.Equal(2, first.Values.Count);
            Assert.Equal("plural form", second.Name);
            Assert.Single(second.Values);

        }

        [Fact]
        public void WordToGetWord_ValuesShouldBeEqualAndNoExceptionThrown()
        {
            Word domain = new()
            {
                ID = 42,
                SourceLanguageName = "language",
                Value = "value",
                Properties = new()
                {
                    new WordProperty
                    {
                        ID = 1,
                        Name = "gender",
                        Values = new("masculine", "feminine")
                    },

                    new WordProperty
                    {
                        ID = 2,
                        Name = "plural",
                        Values = new("values")
                    }
                }
            };

            var dto = sut.Map<Word, GetWord>(domain);
            var first = dto.Properties.First();
            var second = dto.Properties.Last();

            Assert.Equal(42, dto.ID);
            Assert.Equal("value", dto.Value);
            Assert.Equal("language", dto.SourceLanguageName);
            Assert.Equal(2, dto.Properties.Count);
            Assert.Equal("gender", first.Name);
            Assert.Equal(2, first.Values.Count);
            Assert.Equal("plural", second.Name);
            Assert.Single(second.Values);

        }
    }
}
