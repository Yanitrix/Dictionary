using Domain.Dto;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface IWordService
    {
        GetWord Get(int id);

        IEnumerable<GetWord> Get(String wordValue);

        ValidationResult Add(CreateWord dto);

        ValidationResult Update(UpdateWord dto);

        ValidationResult Delete(int id);
    }
}
