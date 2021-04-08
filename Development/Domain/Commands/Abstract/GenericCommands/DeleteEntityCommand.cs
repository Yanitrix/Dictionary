using Domain.Repository;

namespace Domain.Commands
{
    public class DeleteEntityCommand<EntityType, PrimaryKeyType> : ICommand
    {
        public PrimaryKeyType PrimaryKey { get; set; }
    }
    
    public class DeleteEntityCommandHandler<EntityType, PrimaryKeyType> : ICommandHandler<DeleteEntityCommand<EntityType, PrimaryKeyType>, EntityType>
    {
        private readonly IRepository<EntityType, PrimaryKeyType> repo;

        public DeleteEntityCommandHandler(IRepository<EntityType, PrimaryKeyType> repo)
        {
            this.repo = repo;
        }

        public Response<EntityType> Handle(DeleteEntityCommand<EntityType, PrimaryKeyType> command)
        {
            var entity = repo.GetByPrimaryKey(command.PrimaryKey);
            repo.Delete(entity);
            return Response<EntityType>.Ok(entity);
        }
    }
}