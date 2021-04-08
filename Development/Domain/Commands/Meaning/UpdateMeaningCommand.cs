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
    public class UpdateMeaningCommand : ICommand
    {
        public int ID { get; set; }

        public String Value { get; set; }

        public String Notes { get; set; }

        public ICollection<ExampleDto> Examples { get; set; } = new List<ExampleDto>();
    }
    
    public class UpdateMeaningCommandHandler : UpdateEntityCommandHandler<UpdateMeaningCommand, Meaning, int, GetMeaning>
    {
        public UpdateMeaningCommandHandler(IMapper mapper, IMeaningRepository repo) : base(mapper, repo)
        {
        }
    }
}
