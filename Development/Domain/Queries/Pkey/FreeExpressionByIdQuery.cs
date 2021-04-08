using Domain.Dto;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Queries
{
    public class FreeExpressionByIdQuery : EntityByPrimaryKeyQuery<int>
    {
        public FreeExpressionByIdQuery(int primaryKey) : base(primaryKey)
        {
        }
    }
    
    public class FreeExpressionByIdQueryHandler : PrimaryKeyQueryHandler<FreeExpression, int, GetFreeExpression>, IQueryHandler<FreeExpressionByIdQuery, GetFreeExpression>
    {
        public FreeExpressionByIdQueryHandler(IFreeExpressionRepository repo, IMapper mapper) : base(repo, mapper)
        {
        }

        public GetFreeExpression Handle(FreeExpressionByIdQuery query)
        {
            return base.Handle(query);
        }
    }
}