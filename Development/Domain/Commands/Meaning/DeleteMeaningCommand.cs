using Domain.Commands;
using Domain.Models;
using Domain.Repository;

namespace Domain.Dto
{
    public class DeleteMeaningCommand : DeleteEntityCommand<Meaning, int>
    {
    }
    
    public class DeleteMeaningCommandHandler : DeleteEntityCommandHandler<Meaning, int>
    {
        public DeleteMeaningCommandHandler(IMeaningRepository repo) : base(repo)
        {
        }
    }
}