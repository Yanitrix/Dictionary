using Domain.Dto;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Queries
{
    public class DictionaryByIndexQuery : EntityByPrimaryKeyQuery<int>
    {
        public DictionaryByIndexQuery(int primaryKey) : base(primaryKey)
        {
        }
    }

    public class DictionaryByIndexQueryHandler : PrimaryKeyQueryHandler<Dictionary, int, GetDictionary>, IQueryHandler<DictionaryByIndexQuery, GetDictionary>
    {
        public DictionaryByIndexQueryHandler(IDictionaryRepository repo, IMapper mapper) : base(repo, mapper)
        {
        }

        public GetDictionary Handle(DictionaryByIndexQuery query)
        {
            return base.Handle(query);
        }
    }
}