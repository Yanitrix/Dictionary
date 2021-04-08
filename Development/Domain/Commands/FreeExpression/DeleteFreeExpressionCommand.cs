using Domain.Commands;
using Domain.Models;
using Domain.Repository;

namespace Domain.Dto
{
    public class DeleteFreeExpressionCommand : DeleteEntityCommand<FreeExpression, int>
    {
    }
    
    public class DeleteFreeExpressionCommandHandler : DeleteEntityCommandHandler<FreeExpression, int>
    {
        public DeleteFreeExpressionCommandHandler(IFreeExpressionRepository repo) : base(repo)
        {
        }
    }
}