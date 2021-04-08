using Domain.Mapper;
using Domain.Repository;

namespace Domain.Queries
{
    public class EntityByPrimaryKeyQuery<PrimaryKeyType> : IQuery
    {
        public EntityByPrimaryKeyQuery(PrimaryKeyType primaryKey)
        {
            this.PrimaryKey = primaryKey;
        }
        
        public PrimaryKeyType PrimaryKey { get; set; }
    }

    public class PrimaryKeyQueryHandler<EntityType, PrimaryKeyType, ResponseDtoType> : IQueryHandler<EntityByPrimaryKeyQuery<PrimaryKeyType>, ResponseDtoType>
    {
        private readonly IRepository<EntityType, PrimaryKeyType> repo;
        private readonly IMapper mapper;

        public PrimaryKeyQueryHandler(IRepository<EntityType, PrimaryKeyType> repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public ResponseDtoType Handle(EntityByPrimaryKeyQuery<PrimaryKeyType> query)
        {
            var id = query.PrimaryKey;
            var found = repo.GetByPrimaryKey(id);
            return found == null ? default : mapper.Map<EntityType, ResponseDtoType>(found);
        }
    }
}