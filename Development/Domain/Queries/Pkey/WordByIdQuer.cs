using Domain.Dto;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Queries
{
    public class WordByIdQuery : EntityByPrimaryKeyQuery<int>
    {
        public WordByIdQuery(int primaryKey) : base(primaryKey)
        {
        }
    }
    
    public class WordByIdQueryHandler : PrimaryKeyQueryHandler<Word, int, GetWord>, IQueryHandler<WordByIdQuery, GetWord>
    {
        public WordByIdQueryHandler(IWordRepository repo, IMapper mapper) : base(repo, mapper)
        {
        }

        public GetWord Handle(WordByIdQuery query)
        {
            return base.Handle(query);
        }
    }
}