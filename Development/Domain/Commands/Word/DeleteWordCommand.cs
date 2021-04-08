using System.Threading;
using System.Threading.Tasks;
using Domain.Commands;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace Domain.Dto
{
    public class DeleteWordCommand : DeleteEntityCommand<Word, int>
    {
    }
    
    public class DeleteWordCommandHandler : DeleteEntityCommandHandler<Word, int>
    {
        public DeleteWordCommandHandler(IWordRepository repo):base(repo)
        {
        }
    }
}