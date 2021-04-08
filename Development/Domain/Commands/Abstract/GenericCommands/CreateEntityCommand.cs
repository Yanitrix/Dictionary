using Domain.Mapper;
using Domain.Repository;

namespace Domain.Commands
{
    public class CreateEntityCommandHandler<CreateCommandType, EntityType, PrimaryKeyType, ReturnedDtoType>
        : ICommandHandler<CreateCommandType, ReturnedDtoType> where CreateCommandType : ICommand
    {
        protected readonly IMapper mapper;
        protected readonly IRepository<EntityType, PrimaryKeyType> repo;

        protected CreateEntityCommandHandler(IMapper mapper, IRepository<EntityType, PrimaryKeyType> repo)
        {
            this.mapper = mapper;
            this.repo = repo;
        }

        public Response<ReturnedDtoType> Handle(CreateCommandType command)
        {
            var entity = mapper.Map<CreateCommandType, EntityType>(command);
            repo.Create(entity);
            var returnedDto = mapper.Map<EntityType, ReturnedDtoType>(entity);
            return Response<ReturnedDtoType>.Created(returnedDto);
        }
    }
}