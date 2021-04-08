using Domain.Commands;
using Domain.Dto;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace WebUI
{
    public static class CommandHandlersDI
    {
        public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            AddHandlers(services);
            Decorate(services);
            
            return services;
        }

        private static void AddHandlers(IServiceCollection services)
        {
            services.AddTransient<ICommandHandler<CreateDictionaryCommand, GetDictionary>, CreateDictionaryCommandHandler>();
            services.AddTransient<ICommandHandler<DeleteDictionaryCommand, Dictionary>, DeleteDictionaryCommandHandler>();
            
            services.AddTransient<ICommandHandler<CreateEntryCommand, GetEntry>, CreateEntryCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateEntryCommand, GetEntry>, UpdateEntryCommandHandler>();
            services.AddTransient<ICommandHandler<DeleteEntryCommand, Entry>, DeleteEntryCommandHandler>();
            
            services.AddTransient<ICommandHandler<CreateFreeExpressionCommand, GetFreeExpression>, CreateFreeExpressionCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateFreeExpressionCommand, GetFreeExpression>, UpdateFreeExpressionCommandHandler>();
            services.AddTransient<ICommandHandler<DeleteFreeExpressionCommand, FreeExpression>, DeleteFreeExpressionCommandHandler>();
            
            services.AddTransient<ICommandHandler<CreateLanguageCommand, GetLanguage>, CreateLanguageCommandHandler>();
            services.AddTransient<ICommandHandler<DeleteLanguageCommand, Language>, DeleteLanguageCommandHandler>();
            
            services.AddTransient<ICommandHandler<CreateMeaningCommand, GetMeaning>, CreateMeaningCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateMeaningCommand, GetMeaning>, UpdateMeaningCommandHandler>();
            services.AddTransient<ICommandHandler<DeleteMeaningCommand, Meaning>, DeleteMeaningCommandHandler>();
            
            services.AddTransient<ICommandHandler<CreateWordCommand, GetWord>, CreateWordCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateWordCommand, GetWord>, UpdateWordCommandHandler>();
            services.AddTransient<ICommandHandler<DeleteWordCommand, Word>, DeleteWordCommandHandler>();
        }

        private static void Decorate(IServiceCollection services)
        {
            services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));
        }
    }
}