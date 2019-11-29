using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;

namespace Z01.Models
{
  public class MyContext : DbContext
  {
    public DbSet<Note> Notes { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<NoteCategory> NoteCategories { get; set; }

    public MyContext(DbContextOptions<MyContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Note>().Property(_ => _.RowVersion).IsRowVersion();
      modelBuilder.Entity<Note>().Property(_ => _.RowVersion).IsConcurrencyToken();
      modelBuilder.Entity<Note>().HasIndex(_ => _.Title).IsUnique();
      modelBuilder.Entity<Category>().HasIndex(_ => _.Name).IsUnique();

      modelBuilder.Entity<NoteCategory>()
          .HasKey(t => new { NoteId = t.NoteID, CategoryId = t.CategoryID });

      modelBuilder.Entity<Note>().HasIndex(_ => _.Title).IsUnique();
      modelBuilder.Entity<Category>().HasIndex(_ => _.Name).IsUnique();

      modelBuilder.Entity<NoteCategory>()
          .HasKey(t => new { NoteId = t.NoteID, CategoryId = t.CategoryID });

      modelBuilder.Entity<NoteCategory>()
          .HasOne(pt => pt.Note)
          .WithMany(p => p.NoteCategories)
          .HasForeignKey(pt => pt.NoteID);

      modelBuilder.Entity<NoteCategory>()
          .HasOne(pt => pt.Category)
          .WithMany(t => t.NoteCategories)
          .HasForeignKey(pt => pt.CategoryID);
    }
  }
}
