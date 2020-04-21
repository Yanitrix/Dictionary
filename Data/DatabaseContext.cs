using System;
using System.Collections.Generic;
using System.Text;
using Dictionary_MVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Dictionary_MVC.Data
{
    public class DatabaseContext : IdentityDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Dictionary>().HasKey(d => new { d.LanguageInName, d.LanguageOutName });


            //other stuff
            builder.Entity<SpeechPartProperty>().Property(p => p.PossibleValues).
                HasConversion(
                list => JsonConvert.SerializeObject(list),
                list => JsonConvert.DeserializeObject<List<String>>(list));
        }
    }
}
