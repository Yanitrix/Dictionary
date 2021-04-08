using System;
using System.Collections.Generic;
using Domain.Commands;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Dto
{
    public class CreateMeaningCommand : ICommand
    {
        public int EntryID { get; set; }

        public String Value { get; set; }

        public String Notes { get; set; }

        public ICollection<ExampleDto> Examples { get; set; } = new List<ExampleDto>();
    }
    
    public class CreateMeaningCommandHandler :  CreateEntityCommandHandler<CreateMeaningCommand, Meaning, int, GetMeaning>
    {
        public CreateMeaningCommandHandler(IMapper mapper, IMeaningRepository repo) : base(mapper, repo)
        {
        }
    }
}
