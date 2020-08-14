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

        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Expression> Expressions { get; set; }
        public DbSet<Meaning> Meanings { get; set; }

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

            #region dictionaries

            var dict = builder.Entity<Dictionary>().ToTable("Dictionary");
            var entry = builder.Entity<Entry>().ToTable("Entry");
            var expression = builder.Entity<Expression>().ToTable("Expression");
            builder.Entity<Meaning>().ToTable("Meaning");

            dict.Property(d => d.Index).ValueGeneratedOnAdd();
            dict.HasKey(d => new { d.LanguageInName, d.LanguageOutName });

            entry
                .HasOne(entry => entry.Dictionary)
                .WithMany(dictionary => dictionary.Entries)
                .HasForeignKey(entry => entry.DictionaryIndex)
                .HasPrincipalKey(dictionary => dictionary.Index);

            entry
                .HasIndex(entry => entry.WordID)  //a word can be in only one entry, i'm not quite sure if it's the correct way to do it but i think it'll work
                .IsUnique();

            expression
                .HasOne(expression => expression.Dictionary)
                .WithMany(dictionary => dictionary.FreeExpressions)
                .HasForeignKey(expression => expression.DictionaryIndex)
                .HasPrincipalKey(dictionary => dictionary.Index);

            #endregion


        }
    }
}
