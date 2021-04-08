using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Dto;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Queries
{
    public class EntryByWordAndDictionaryQuery : IQuery
    {
        public String WordValue { get; set; }
        public int? DictionaryIndex { get; set; }
    }
    
    public class EntryByWordAndDictionaryQueryHandler : IQueryHandler<EntryByWordAndDictionaryQuery, IEnumerable<GetEntry>>
    {
        private readonly IEntryRepository repo;
        private readonly IMapper mapper;

        public EntryByWordAndDictionaryQueryHandler(IEntryRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public IEnumerable<GetEntry> Handle(EntryByWordAndDictionaryQuery query)
        {
            var (word, index) = (query.WordValue, query.DictionaryIndex);
            
            if (word == null && index == null)
                return Array.Empty<GetEntry>();
            if (word != null && index != null)
                return repo.GetByDictionaryAndWord(index.Value, word, false).Select(Map).ToList();
            if (word != null)
                return repo.GetByWord(word, false).Select(Map).ToList();
            
            return repo.GetByDictionary(index.Value).Select(Map).ToList();
        }
        
        private GetEntry Map(Entry obj) => mapper.Map<Entry, GetEntry>(obj);
    }
}