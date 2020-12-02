using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Data.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using FluentValidation;
using Data.Models;
using Data.Mapper;
using Service.Validation;
using Service.Repository;
using Service;

namespace Dictionary_MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("ConnectionString")));

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<DatabaseContext>();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSingleton<AbstractValidator<Language>, LanguageValidator>();
            services.AddSingleton<AbstractValidator<Word>, WordValidator>();
            services.AddSingleton<AbstractValidator<WordProperty>, WordPropertyValidator>();

            services.AddSingleton<AbstractValidator<Dictionary>, DictionaryValidator>();
            services.AddSingleton<AbstractValidator<Entry>, EntryValidator>();
            services.AddSingleton<AbstractValidator<Meaning>, MeaningValidator>();
            services.AddSingleton<AbstractValidator<Example>, ExampleValidator>();
            services.AddSingleton<AbstractValidator<FreeExpression>, FreeExpressionValidator>();

            services.AddTransient<ILanguageRepository, LanguageRepository>();
            services.AddTransient<IWordRepository, WordRepository>();
            services.AddTransient<IDictionaryRepository, DictionaryRepository>();
            services.AddTransient<IEntryRepository, EntryRepository>();
            services.AddTransient<IMeaningRepository, MeaningRepository>();
            services.AddTransient<IExampleRepository, ExampleRepository>();
            services.AddTransient<IFreeExpressionRepository, FreeExpressionRepository>();

            services.AddTransient<IService<Language>, LanguageService>();
            services.AddTransient<IService<Word>, WordService>();
            services.AddTransient<IService<Dictionary>, DictionaryService>();
            services.AddTransient<IService<Entry>, EntryService>();
            services.AddTransient<IService<Meaning>, MeaningService>();
            services.AddTransient<IService<FreeExpression>, FreeExpressionService>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(typeof(MapperProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
