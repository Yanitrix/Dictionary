using System;
using Domain.Dto;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Queries
{
    public class LanguageByNameQuery : EntityByPrimaryKeyQuery<String>
    {
        public LanguageByNameQuery(string primaryKey) : base(primaryKey)
        {
        }
    }
    
    public class LanguageByNameQueryHandler : PrimaryKeyQueryHandler<Language, String, GetLanguage>, IQueryHandler<LanguageByNameQuery, GetLanguage>
    {
        public LanguageByNameQueryHandler(ILanguageRepository repo, IMapper mapper) : base(repo, mapper)
        {
        }

        public GetLanguage Handle(LanguageByNameQuery query)
        {
            return base.Handle(query);
        }
    }
}