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
        private WordRepository service;

        public WordServiceTests()
        {
            service = new WordRepository(this.context);

            putData();
        }

        private void putData()
        {

        }


    }
}
