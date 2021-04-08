using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Commands;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Dto
{
    public class DeleteLanguageCommand : DeleteEntityCommand<Language, String>
    {
    }
    
    public class DeleteLanguageCommandHandler : DeleteEntityCommandHandler<Language, String>
    {
        public DeleteLanguageCommandHandler(ILanguageRepository repo) : base(repo)
        {
        }
    }
}