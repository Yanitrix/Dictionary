using System;
using System.Collections.Generic;
using Domain.Commands;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Dto
{
    public class CreateWordCommand : ICommand
    {
        public String Value { get; set; }
 
        public String SourceLanguageName { get; set; }

        public ISet<WordPropertyDto> Properties { get; set; } = new HashSet<WordPropertyDto>();
    }
    
    public class CreateWordCommandHandler : CreateEntityCommandHandler<CreateWordCommand, Word, int, GetWord>
    {
        public CreateWordCommandHandler(IMapper mapper, IWordRepository repo) : base(mapper, repo)
        {
        }
    }
}
