using Domain.Dto;
using Domain.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace WebUI
{
    public static class ValidatorsDI
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddTransient<AbstractValidator<CreateDictionaryCommand>, CreateDictionaryValidator>();
            services.AddTransient<AbstractValidator<DeleteDictionaryCommand>, DeleteDictionaryValidator>();
            
            services.AddTransient<AbstractValidator<CreateFreeExpressionCommand>, CreateFreeExpressionValidator>();
            services.AddTransient<AbstractValidator<UpdateFreeExpressionCommand>, UpdateFreeExpressionValidator>();
            services.AddTransient<AbstractValidator<DeleteFreeExpressionCommand>, DeleteFreeExpressionValidator>();
            services.AddTransient<AbstractValidator<CreateEntryCommand>, CreateEntryValidator>();
            services.AddTransient<AbstractValidator<UpdateEntryCommand>, UpdateEntryValidator>();
            services.AddTransient<AbstractValidator<DeleteEntryCommand>, DeleteEntryValidator>();
            
            services.AddTransient<AbstractValidator<CreateLanguageCommand>, CreateLanguageValidator>();
            services.AddTransient<AbstractValidator<DeleteLanguageCommand>, DeleteLanguageValidator>();
            
            services.AddTransient<AbstractValidator<CreateMeaningCommand>, CreateMeaningValidator>();
            services.AddTransient<AbstractValidator<UpdateMeaningCommand>, UpdateMeaningValidator>();
            services.AddTransient<AbstractValidator<DeleteMeaningCommand>, DeleteMeaningValidator>();
            
            services.AddTransient<AbstractValidator<CreateWordCommand>, CreateWordValidator>();
            services.AddTransient<AbstractValidator<UpdateWordCommand>, UpdateWordValidator>();
            services.AddTransient<AbstractValidator<DeleteWordCommand>, DeleteWordValidator>();
            services.AddTransient<AbstractValidator<WordPropertyDto>, WordPropertyDtoValidator>();

            return services;
        }
    }
}