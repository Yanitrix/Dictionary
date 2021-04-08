using Domain.Commands;
using Domain.Models;
using Domain.Repository;

namespace Domain.Dto
{
    public class DeleteEntryCommand : DeleteEntityCommand<Entry, int>
    {
    }
    
    public class DeleteEntryCommandHandler : DeleteEntityCommandHandler<Entry, int>
    {
        public DeleteEntryCommandHandler(IEntryRepository repo) : base(repo)
        {
        }
    }
}