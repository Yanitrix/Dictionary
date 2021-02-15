using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Database
{
    public class DatabaseContext : IdentityDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Language> Languages { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<WordProperty> WordProperties { get; set; }

        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Meaning> Meanings { get; set; }
        public DbSet<Example> Examples { get; set; }
        public DbSet<FreeExpression> FreeExpressions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region languages and words and wordproperties

            builder.Entity<Language>()
                .ToTable("Language")
                .Property(language => language.Name)
                .UseCollation("SQL_Latin1_General_CP1_CS_AS");

            var word = builder.Entity<Word>().ToTable("Word");

            word.Property(word => word.SourceLanguageName)
                .UseCollation("SQL_Latin1_General_CP1_CS_AS");

            word
                .HasMany<Entry>()
                .WithOne(entry => entry.Word)
                .HasForeignKey(entry => entry.WordID)
                .OnDelete(DeleteBehavior.Cascade); //deleting all entries that contain given word

            word
                .Property(word => word.Value)
                .UseCollation("SQL_Latin1_General_CP1_CS_AS");

            builder.Entity<WordProperty>().ToTable("WordProperty");

            //storing StringSet
            var propertyBuilder = builder.Entity<WordProperty>().Property(wp => wp.Values);
            propertyBuilder.HasConversion(StringSetValueComparerAndConverter.Converter);
            propertyBuilder.Metadata.SetValueConverter(StringSetValueComparerAndConverter.Converter);
            propertyBuilder.Metadata.SetValueComparer(StringSetValueComparerAndConverter.Comparer);

            #endregion

            #region dictionaries

            var dictionary = builder.Entity<Dictionary>().ToTable("Dictionary");
            var entry = builder.Entity<Entry>().ToTable("Entry");
            var example = builder.Entity<Example>().ToTable("Example");
            var freeExpression = builder.Entity<FreeExpression>().ToTable("FreeExpression");
            var meaning = builder.Entity<Meaning>().ToTable("Meaning");

            dictionary.Property(d => d.Index).ValueGeneratedOnAdd();
            dictionary.HasKey(d => new { d.LanguageInName, d.LanguageOutName });

            dictionary.Property(dictionary => dictionary.LanguageInName)
                .UseCollation("SQL_Latin1_General_CP1_CS_AS");

            dictionary
                .HasMany(dictionary => dictionary.Entries)
                .WithOne(entry => entry.Dictionary)
                .HasForeignKey(entry => entry.DictionaryIndex)
                .HasPrincipalKey(dictionary => dictionary.Index)
                .OnDelete(DeleteBehavior.Cascade);

            dictionary.Property(dictionary => dictionary.LanguageOutName)
                .UseCollation("SQL_Latin1_General_CP1_CS_AS");

            dictionary
                .HasMany(dictionary => dictionary.FreeExpressions)
                .WithOne(freeExpression => freeExpression.Dictionary)
                .HasForeignKey(freeExpression => freeExpression.DictionaryIndex)
                .HasPrincipalKey(dictionary => dictionary.Index)
                .OnDelete(DeleteBehavior.Cascade);

            dictionary
                .HasOne(dictionary => dictionary.LanguageIn)
                .WithMany()
                .HasForeignKey(dictionary => dictionary.LanguageInName)
                .OnDelete(DeleteBehavior.Restrict);

            dictionary
                .HasOne(dictionary => dictionary.LanguageOut)
                .WithMany()
                .HasForeignKey(dictionary => dictionary.LanguageOutName)
                .OnDelete(DeleteBehavior.Restrict);

            //well, it didn't work. Entry uniqueness should be determined upon its WordID and DictionaryIndex.
            //The combination of those should be unique
            entry
                .HasIndex(entry => new { entry.WordID, entry.DictionaryIndex })  //there can be only one Entry in each Dictionary referring to a certain Word
                .IsUnique();

            #endregion

        }
    }
}
