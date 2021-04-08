using Domain.Commands;
using Domain.Models;
using Domain.Repository;

namespace Domain.Dto
{
    public class DeleteDictionaryCommand : DeleteEntityCommand<Dictionary, int>
    {
    }
    
    public class DeleteDictionaryCommandHandler : DeleteEntityCommandHandler<Dictionary, int>
    {
        public DeleteDictionaryCommandHandler(IDictionaryRepository repo) : base(repo)
        {
        }
    }
}