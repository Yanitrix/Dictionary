using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Dto;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Queries
{
    public class WordByValueQuery : IQuery
    {
        public WordByValueQuery(String value)
        {
            Value = value;
        }
        
        public String Value { get; set; }
    }
    
    public class WordByValueQueryHandler : IQueryHandler<WordByValueQuery, IEnumerable<GetWord>>
    {
        private readonly IWordRepository repo;
        private readonly IMapper mapper;

        public WordByValueQueryHandler(IWordRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public IEnumerable<GetWord> Handle(WordByValueQuery query)
        {
            return query.Value == null ? Array.Empty<GetWord>() : repo.GetByValue(query.Value, false).Select(mapper.Map<Word, GetWord>);
        }
    }
}