using Domain.Dto;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Queries
{
    public class MeaningByIdQuery : EntityByPrimaryKeyQuery<int>
    {
        public MeaningByIdQuery(int primaryKey) : base(primaryKey)
        {
        }
    }
    
    public class MeaningByIdQueryHandler : PrimaryKeyQueryHandler<Meaning, int, GetMeaning>, IQueryHandler<MeaningByIdQuery, GetMeaning>
    {
        public MeaningByIdQueryHandler(IMeaningRepository repo, IMapper mapper) : base(repo, mapper)
        {
        }

        public GetMeaning Handle(MeaningByIdQuery query)
        {
            return base.Handle(query);
        }
    }
}