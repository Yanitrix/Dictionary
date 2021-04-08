using Domain.Dto;
using Domain.Queries;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace WebUI
{
    public static class QueryHandlersDI
    {
        public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
        {
            services.AddTransient<IQueryHandler<DictionaryByIndexQuery, GetDictionary>, DictionaryByIndexQueryHandler>();
            services.AddTransient<IQueryHandler<EntryByIdQuery, GetEntry>, EntryByIdQueryHandler>();
            services.AddTransient<IQueryHandler<FreeExpressionByIdQuery, GetFreeExpression>, FreeExpressionByIdQueryHandler>();
            services.AddTransient<IQueryHandler<LanguageByNameQuery, GetLanguage>, LanguageByNameQueryHandler>();
            services.AddTransient<IQueryHandler<MeaningByIdQuery, GetMeaning>, MeaningByIdQueryHandler>();
            services.AddTransient<IQueryHandler<WordByIdQuery, GetWord>, WordByIdQueryHandler>();

            services.AddTransient<IQueryHandler<DictionaryContainingLanguagesQuery, IEnumerable<GetDictionary>>, DictionaryContainingLanguagesQueryHandler>();
            services.AddTransient<IQueryHandler<EntryByWordAndDictionaryQuery, IEnumerable<GetEntry>>, EntryByWordAndDictionaryQueryHandler>();
            services.AddTransient<IQueryHandler<WordByValueQuery, IEnumerable<GetWord>>, WordByValueQueryHandler>();

            return services;
        }
    }
}
