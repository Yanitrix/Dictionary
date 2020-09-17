using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    public interface IWordPropertyRepository : IRepository<WordProperty>
    {
        public WordProperty GetByID(int id);
    }
}
