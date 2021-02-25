using Domain.Dto;

namespace Service
{
    public interface IMeaningService
    {
        GetMeaning Get(int id);

        ValidationResult Add(CreateMeaning dto);

        ValidationResult Update(UpdateMeaning dto);

        ValidationResult Delete(int id);
    }
}
