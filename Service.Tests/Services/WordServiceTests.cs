using Api.Service;
using Api.Service.Validation;
using Data.Tests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Tests.Services
{
    public class WordServiceTests : DbContextTestBase
    {
        private WordService service;

        public WordServiceTests()
        {
            service = new WordService(this.context, new WordValidator());
            service.ValidationDictionary = new ValidationDictionary();

            putData();
        }

        private void putData()
        {

        }


    }
}
