using Data.Dto;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    public interface ILanguageService : IService<Language>
    {
        Language GetByName(String name);

    }
}
