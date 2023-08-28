using Microsoft.EntityFrameworkCore;
using Synonym_Test.Models;

namespace Synonyms_Test.DataAccess.Context;

public  class WordContext : DbContext
{
    public DbSet<Word> Words => Set<Word>();

    public WordContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Word>()
            .HasKey(w => w.WordId); // Define primary key

        modelBuilder.Entity<Word>()
            .HasMany(w => w.Synonyms)
            .WithMany(); // Configure many-to-many relationship for Synonyms
            
            
        base.OnModelCreating(modelBuilder);
    }
}