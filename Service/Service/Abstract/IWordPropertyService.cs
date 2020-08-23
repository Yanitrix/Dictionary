using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    public interface IWordPropertyService : IService<WordProperty>
    {
        public WordProperty GetByID(int id);
    }
}
