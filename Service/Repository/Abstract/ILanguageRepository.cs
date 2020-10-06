using Data.Models;
using System;

namespace Service.Repository
{
    public interface ILanguageRepository : IRepository<Language>
    {
        Language GetByName(String name);

        bool ExistsByName(String name);
    }
}
