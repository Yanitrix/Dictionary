using Domain.Dto;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Queries
{
    public class EntryByIdQuery : EntityByPrimaryKeyQuery<int>
    {
        public EntryByIdQuery(int primaryKey) : base(primaryKey)
        {
        }
    }

    public class EntryByIdQueryHandler : PrimaryKeyQueryHandler<Entry, int, GetEntry>, IQueryHandler<EntryByIdQuery, GetEntry>
    {
        public EntryByIdQueryHandler(IEntryRepository repo, IMapper mapper) : base(repo, mapper)
        {
        }

        public GetEntry Handle(EntryByIdQuery query)
        {
            return base.Handle(query);
        }
    }
}