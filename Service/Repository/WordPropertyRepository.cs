using Data.Database;
using Data.Models;

namespace Service.Repository
{
    public class WordPropertyRepository : RepositoryBase<WordProperty>, IWordPropertyRepository
    {
        public WordPropertyRepository(DatabaseContext context):base(context) { }

        public WordProperty GetByID(int id)
        {
            return repo.Find(id);
        }

    }
}
