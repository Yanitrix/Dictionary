using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Data.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Data.Models;
using Data.Mapper;
using Service.Repository;
using Service;
using Application.Filters;
using FluentValidation.AspNetCore;
using Service.Service;

namespace Application
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

            services.AddMvc(opt =>
            {
                opt.Filters.Add<ValidationActionFilter>();
            }).AddFluentValidation(opt =>
            {
                opt.RegisterValidatorsFromAssemblyContaining<Startup>();
                opt.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            });

            services.AddTransient<ILanguageService, LanguageService>();
            services.AddTransient<IWordService, WordService>();
            services.AddTransient<IDictionaryService, DictionaryService>();
            services.AddTransient<IEntryService, EntryService>();
            services.AddTransient<IMeaningService, MeaningService>();
            services.AddTransient<IFreeExpressionService, FreeExpressionService>();
            services.AddTransient<ITranslationService, TranslationService>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddSimpleMapper<MappingConfig>();
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
