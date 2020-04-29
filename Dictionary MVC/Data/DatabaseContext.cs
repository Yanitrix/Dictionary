﻿using System;
using System.Collections.Generic;
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
            base.OnModelCreating(builder);

            builder.Entity<Dictionary>().HasKey(d => new { d.LanguageInName, d.LanguageOutName });

            builder.Entity<SpeechPart>().HasKey(s => new { s.LanguageName, s.Name });

            //other stuff
            builder.Entity<SpeechPartProperty>().Property(p => p.PossibleValues).
                HasConversion(
                list => JsonConvert.SerializeObject(list),
                list => JsonConvert.DeserializeObject<List<String>>(list));
        }
    }
}
