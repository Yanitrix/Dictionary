using Domain.Commands;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Dto
{
    public class CreateEntryCommand : ICommand
    {
        public int DictionaryIndex { get; set; }

        public int WordID { get; set; }
    }
    
    public class CreateEntryCommandHandler : CreateEntityCommandHandler<CreateEntryCommand, Entry, int, GetEntry>
    {
        public CreateEntryCommandHandler(IMapper mapper, IEntryRepository repo) : base(mapper, repo)
        {
        }
    }
}
