using Data.Models;

namespace Service
{
    public interface IFreeExpressionService : IService<FreeExpression>
    {
        FreeExpression Get(int id);

        ValidationResult Delete(int id);
    }
}
