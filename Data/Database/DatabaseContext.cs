using Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Database
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

            builder.Entity<Language>().ToTable("Language");
            var word = builder.Entity<Word>().ToTable("Word");

            word
                .HasOne<Entry>()
                .WithOne(entry => entry.Word)
                .HasForeignKey<Entry>(entry => entry.WordID)
                .OnDelete(DeleteBehavior.Cascade); //deleting all entries that contain given word

            builder.Entity<WordProperty>().ToTable("WordProperty");

            //storing Set<String>
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

            entry
                .HasIndex(entry => entry.WordID)  //a word can be in only one entry, i'm not quite sure if it's the correct way to do it but i think it'll work
                .IsUnique();

            #endregion

        }
    }
}
