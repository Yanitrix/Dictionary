using Domain.Models;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface IWordService : IService<Word>
    {
        Word Get(int id);

        IEnumerable<Word> Get(String wordValue);

        ValidationResult Delete(int id);
    }
}
