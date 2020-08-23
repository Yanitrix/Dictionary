using System;
using System.Collections.Generic;
using Data;
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

            var speechPart = builder.Entity<SpeechPart>().ToTable("SpeechPart");
            var speechPartProperty = builder.Entity<SpeechPartProperty>().ToTable("SpeechPartProperty");
            var wordProperty = builder.Entity<WordProperty>().ToTable("WordProperty");


            speechPart.HasKey(s => new { s.LanguageName, s.Name });
            speechPart.Property(part => part.Index).ValueGeneratedOnAdd();

            speechPart
                .HasMany(speechPart => speechPart.Properties)
                .WithOne(property => property.SpeechPart)
                .HasForeignKey(property => property.SpeechPartIndex)
                .HasPrincipalKey(speechPart => speechPart.Index)
                .OnDelete(DeleteBehavior.Cascade);

            //storing Set<String>
            var propertyBuilder = builder.Entity<SpeechPartProperty>().Property(p => p.PossibleValues);
            propertyBuilder.HasConversion(StringSetValueComparerAndConverter.Converter);
            propertyBuilder.Metadata.SetValueConverter(StringSetValueComparerAndConverter.Converter);
            propertyBuilder.Metadata.SetValueComparer(StringSetValueComparerAndConverter.Comparer);


            #endregion

            #region languages and words

            var language = builder.Entity<Language>().ToTable("Language");
            var word = builder.Entity<Word>().ToTable("Word");

            language
                .HasMany(language => language.Words)
                .WithOne(word => word.SourceLanguage)
                .HasForeignKey(word => word.SourceLanguageName)
                .OnDelete(DeleteBehavior.Cascade);

            word
                .HasOne<Entry>()
                .WithOne(entry => entry.Word)
                .HasForeignKey<Entry>(entry => entry.WordID)
                .OnDelete(DeleteBehavior.Cascade); //deleting all entries that contain given word

            word
                .HasMany(word => word.Properties)
                .WithOne(property => property.Word)
                .HasForeignKey(property => property.WordID)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region dictionaries

            var dictionary = builder.Entity<Dictionary>().ToTable("Dictionary");
            var entry = builder.Entity<Entry>().ToTable("Entry");
            var expression = builder.Entity<Expression>().ToTable("Expression");
            var meaning = builder.Entity<Meaning>().ToTable("Meaning");

            dictionary.Property(d => d.Index).ValueGeneratedOnAdd();
            dictionary.HasKey(d => new { d.LanguageInName, d.LanguageOutName });

            dictionary
                .HasMany(dictionary => dictionary.Entries)
                .WithOne(entry => entry.Dictionary)
                .HasForeignKey(entry => entry.DictionaryIndex)
                .HasPrincipalKey(dictionary => dictionary.Index)
                .OnDelete(DeleteBehavior.Cascade);

            dictionary
                .HasMany(dictionary => dictionary.FreeExpressions)
                .WithOne(freeExpression => freeExpression.Dictionary)
                .HasForeignKey(freeExpression => freeExpression.DictionaryIndex)
                .HasPrincipalKey(dictionary => dictionary.Index)
                .OnDelete(DeleteBehavior.Restrict);


            //TODO put pressure on testing if these dictionaries get deleted in a proper way. https://github.com/Yanitrix/Dictionary-MVC/issues/80
            dictionary
                .HasOne(dictionary => dictionary.LanguageIn)
                .WithMany()
                .HasForeignKey(dictionary => dictionary.LanguageInName)
                .OnDelete(DeleteBehavior.Cascade);

            dictionary
                .HasOne(dictionary => dictionary.LanguageOut)
                .WithMany()
                .HasForeignKey(dictionary => dictionary.LanguageOutName)
                .OnDelete(DeleteBehavior.Cascade);


            entry
                .HasMany(entry => entry.Meanings)
                .WithOne(meaning => meaning.Entry)
                .HasForeignKey(meaning => meaning.EntryID)
                .OnDelete(DeleteBehavior.Cascade);

            entry
                .HasIndex(entry => entry.WordID)  //a word can be in only one entry, i'm not quite sure if it's the correct way to do it but i think it'll work
                .IsUnique();

            meaning
                .HasMany(meaning => meaning.Examples)
                .WithOne(example => example.Meaning)
                .HasForeignKey(example => example.MeaningID)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion


        }
    }
}
