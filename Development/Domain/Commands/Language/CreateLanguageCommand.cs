using System;
using Domain.Commands;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Dto
{
    public class CreateLanguageCommand : ICommand
    {
        public String Name { get; set; }
    }
    
    public class CreateLanguageCommandHandler : CreateEntityCommandHandler<CreateLanguageCommand, Language, String, GetLanguage>
    {
        public CreateLanguageCommandHandler(IMapper mapper, ILanguageRepository repo) : base(mapper, repo)
        {
        }
    }
}
