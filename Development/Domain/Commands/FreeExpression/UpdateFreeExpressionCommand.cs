using System;
using Domain.Commands;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Dto
{
    public class UpdateFreeExpressionCommand : ICommand
    {
        public int DictionaryIndex { get; set; }

        public String Text { get; set; }

        public String Translation { get; set; }
        
        public int ID { get; set; }
    }
    
    public class UpdateFreeExpressionCommandHandler : UpdateEntityCommandHandler<UpdateFreeExpressionCommand, FreeExpression, int, GetFreeExpression>
    {
        public UpdateFreeExpressionCommandHandler(IMapper mapper, IFreeExpressionRepository repo) : base(mapper, repo)
        {
        }
    }
}
