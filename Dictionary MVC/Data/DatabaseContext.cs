using System;
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

        public DbSet<SpeechPart> SpeechParts { get; set; }
        public DbSet<SpeechPartProperty> SpeechPartProperties { get; set; }
        public DbSet<WordProperty> WordProperties { get; set; }

        public DbSet<Word> Words { get; set; }
        public DbSet<Language> Languages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region speech properties

            builder.Entity<SpeechPart>().ToTable("SpeechPart");
            builder.Entity<SpeechPartProperty>().ToTable("SpeechPartProperty");
            builder.Entity<WordProperty>().ToTable("WordProperty");
            

            builder.Entity<SpeechPart>().HasKey(s => new { s.LanguageName, s.Name });
            builder.Entity<SpeechPart>().Property(part => part.Index).ValueGeneratedOnAdd();

            builder
                .Entity<SpeechPartProperty>()
                .HasOne(property => property.SpeechPart)
                .WithMany(part => part.Properties)
                .HasForeignKey(property => property.SpeechPartIndex)
                .HasPrincipalKey(part => part.Index);

            //storing List<String>
            builder.Entity<SpeechPartProperty>().Property(p => p.PossibleValues).
                HasConversion(
                list => JsonConvert.SerializeObject(list),
                list => JsonConvert.DeserializeObject<List<String>>(list));

            #endregion

            #region languages and words

            builder.Entity<Language>().ToTable("Language");
            builder.Entity<Word>().ToTable("Word");

            #endregion

            
        }
    }
}
