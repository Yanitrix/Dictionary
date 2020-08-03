using Dictionary_MVC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service.Abstract
{
    interface IEntryService : IService<Entry>
    {
        public Entry GetByID(int id);

        public IEnumerable<Entry> GetByDictionaryAndWord(int dictionaryIndex, String wordValue);
    }
}
