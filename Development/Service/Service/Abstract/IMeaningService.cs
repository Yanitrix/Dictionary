using Domain.Dto;
using Domain.Models;

namespace Service
{
    public interface IMeaningService
    {
        Meaning Get(int id);

        ValidationResult Add(CreateMeaning dto);

        ValidationResult Update(UpdateMeaning dto);

        ValidationResult Delete(int id);
    }
}
