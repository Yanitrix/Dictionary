using System;
using Domain.Commands;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Dto
{
    public class CreateFreeExpressionCommand : ICommand
    {
        public int DictionaryIndex { get; set; }

        public String Text { get; set; }

        public String Translation { get; set; }
    }

    public class CreateFreeExpressionCommandHandler : CreateEntityCommandHandler<CreateFreeExpressionCommand, FreeExpression, int, GetFreeExpression>
    {
        public CreateFreeExpressionCommandHandler(IMapper mapper, IFreeExpressionRepository repo) : base(mapper, repo)
        {
        }
    }
}
