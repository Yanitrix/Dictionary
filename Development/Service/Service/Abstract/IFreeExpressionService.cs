using Domain.Dto;
using Domain.Models;

namespace Service
{
    public interface IFreeExpressionService
    {
        FreeExpression Get(int id);

        ValidationResult Add(CreateOrUpdateFreeExpression dto);

        ValidationResult Update(CreateOrUpdateFreeExpression dto);

        ValidationResult Delete(int id);
    }
}
