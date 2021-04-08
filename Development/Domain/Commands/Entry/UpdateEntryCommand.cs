using Domain.Commands;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Dto
{
    public class UpdateEntryCommand : ICommand
    {
        public int DictionaryIndex { get; set; }

        public int WordID { get; set; }
        
        public int ID { get; set; }
    }
    
    public class UpdateEntryCommandHandler : UpdateEntityCommandHandler<UpdateEntryCommand, Entry, int, GetEntry>
    {
        public UpdateEntryCommandHandler(IMapper mapper, IEntryRepository repo) : base(mapper, repo)
        {
        }
    }
}
