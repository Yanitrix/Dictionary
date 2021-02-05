using Data.Models;

namespace Service
{
    public interface IMeaningService : IService<Meaning>
    {
        Meaning Get(int id);

        ValidationResult Delete(int id);
    }
}
