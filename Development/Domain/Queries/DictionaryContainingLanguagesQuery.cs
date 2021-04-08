using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Dto;
using Domain.Mapper;
using Domain.Models;
using Domain.Repository;

namespace Domain.Queries
{
    public class DictionaryContainingLanguagesQuery : IQuery
    {
        public String Language { get; set; }
        public String LanguageIn { get; set; }
        public String LanguageOut { get; set; }
    }
    
    public class DictionaryContainingLanguagesQueryHandler : IQueryHandler<DictionaryContainingLanguagesQuery, IEnumerable<GetDictionary>>
    {
        private readonly IDictionaryRepository repo;
        private readonly IMapper mapper;

        public DictionaryContainingLanguagesQueryHandler(IDictionaryRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public IEnumerable<GetDictionary> Handle(DictionaryContainingLanguagesQuery query)
        {
            var (lang, langIn, langOut) = (query.Language, query.LanguageIn, query.LanguageOut);

            //none present
            if (lang == null && langIn == null && langOut == null)
                return repo.All().Select(Map).ToList();
            //langIn and langOut present
            if (lang == null && langIn != null && langOut != null)
            {
                var found = repo.GetByLanguageInAndOut(langIn, langOut);
                if (found == null)
                    return Array.Empty<GetDictionary>();
                return new[] { Map(found) };
            }
            //only langIn
            if (lang == null && langIn != null && langOut == null)
                return repo.GetAllByLanguageIn(langIn).Select(Map).ToList();
            //only langOut
            if (lang == null && langIn == null && langOut != null)
                return repo.GetAllByLanguageOut(langOut).Select(Map).ToList();
            //only lang
            if (lang != null && langIn == null && langOut == null)
                return repo.GetAllByLanguage(lang).Select(Map).ToList();

            //everything esle
            return Array.Empty<GetDictionary>();
        }
        
        private GetDictionary Map(Dictionary entity) => mapper.Map<Dictionary, GetDictionary>(entity);
    }
}