using Data.Models;

namespace Service.Repository
{
    public interface IWordPropertyRepository : IRepository<WordProperty>
    {
        public WordProperty GetByID(int id);
    }
}
