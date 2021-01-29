using Data.Models;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface IWordService : IService<Word>
    {
        Word Get(int id);

        IEnumerable<Word> Get(String wordValue);

        IValidationDictionary Delete(int id);
    }
}
