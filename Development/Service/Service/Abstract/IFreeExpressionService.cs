using Domain.Dto;

namespace Service
{
    public interface IFreeExpressionService
    {
        GetFreeExpression Get(int id);

        ValidationResult Add(CreateFreeExpression dto);

        ValidationResult Update(UpdateFreeExpression dto);

        ValidationResult Delete(int id);
    }
}
