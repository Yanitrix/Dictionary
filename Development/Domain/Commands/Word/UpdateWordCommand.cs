using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Commands;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Dto
{
    public class UpdateWordCommand : ICommand
    {
        public int ID { get; set; }

        public String Value { get; set; }

        public ISet<WordPropertyDto> Properties { get; set; } = new HashSet<WordPropertyDto>();
    }
    
    public class UpdateWordCommandHandler :  UpdateEntityCommandHandler<UpdateWordCommand, Word, int, GetWord>
    {
        public UpdateWordCommandHandler(IMapper mapper, IWordRepository repo):base(mapper, repo)
        {
        }
    }
}
