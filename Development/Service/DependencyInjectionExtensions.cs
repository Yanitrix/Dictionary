using Microsoft.Extensions.DependencyInjection;
using Service.Mapper;

namespace Service
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddSimpleMapper<MappingConfig>();

            services.AddTransient<ILanguageService, LanguageService>();
            services.AddTransient<IWordService, WordService>();
            services.AddTransient<IDictionaryService, DictionaryService>();
            services.AddTransient<IEntryService, EntryService>();
            services.AddTransient<IMeaningService, MeaningService>();
            services.AddTransient<IFreeExpressionService, FreeExpressionService>();
            services.AddTransient<ITranslationService, TranslationService>();

            return services;
        }
    }
}
