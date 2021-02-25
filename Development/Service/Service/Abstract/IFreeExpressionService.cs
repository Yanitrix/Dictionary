using Domain.Dto;
using Domain.Models;

namespace Service
{
    public interface IFreeExpressionService
    {
        FreeExpression Get(int id);

        ValidationResult Add(CreateFreeExpression dto);

        ValidationResult Update(UpdateFreeExpression dto);

        ValidationResult Delete(int id);
    }
}
