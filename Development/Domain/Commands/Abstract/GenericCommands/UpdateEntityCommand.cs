using Domain.Mapper;
using Domain.Repository;

namespace Domain.Commands
{
    public class UpdateEntityCommandHandler<UpdateEntityCommandType, EntityType, PrimaryKeyType, ReturnedDtoType>
        : ICommandHandler<UpdateEntityCommandType,  ReturnedDtoType> where UpdateEntityCommandType : ICommand
    {
        private readonly IMapper mapper;
        private readonly IRepository<EntityType, PrimaryKeyType> repo;

        protected UpdateEntityCommandHandler(IMapper mapper, IRepository<EntityType, PrimaryKeyType> repo)
        {
            this.mapper = mapper;
            this.repo = repo;
        }

        public Response<ReturnedDtoType> Handle(UpdateEntityCommandType command)
        {
            var entity = mapper.Map<UpdateEntityCommandType, EntityType>(command);
            repo.Update(entity);
            var returnedDto = mapper.Map<EntityType, ReturnedDtoType>(entity);
            return Response<ReturnedDtoType>.Ok(returnedDto);
        }
    }
}   