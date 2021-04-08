using System;
using Domain.Commands;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Dto
{
    public class CreateDictionaryCommand : ICommand
    {
        public String LanguageIn { get; set; }

        public String LanguageOut { get; set; }
    }

    public class CreateDictionaryCommandHandler : CreateEntityCommandHandler<CreateDictionaryCommand, Dictionary, int, GetDictionary>
    {
        public CreateDictionaryCommandHandler(IMapper mapper, IDictionaryRepository repo) : base(mapper, repo)
        {
        }
    }
}
