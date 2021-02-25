using Domain.Dto;
using Domain.Models;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface IWordService
    {
        Word Get(int id);

        IEnumerable<Word> Get(String wordValue);

        ValidationResult Add(CreateWord dto);

        ValidationResult Update(UpdateWord dto);

        ValidationResult Delete(int id);
    }
}
