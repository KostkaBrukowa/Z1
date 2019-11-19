using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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
      modelBuilder.Entity<NoteCategory>()
          .HasKey(t => new { NoteId = t.NoteID, CategoryId = t.CategoryID });

      modelBuilder.Entity<Note>().Property(_ => _.RowVersion).IsRowVersion();
      modelBuilder.Entity<Note>().Property(_ => _.RowVersion).IsConcurrencyToken();

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


  [Table("Note")]
  public class Note
  {
    public int NoteID { get; set; }
    public DateTime NoteDate { get; set; } = DateTime.Now;
    public bool Markdown { get; set; }
    [Required]
    [MinLength(1), MaxLength(64)]
    public string Title { get; set; }
    [MinLength(1)]
    public string Content { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; }
    public List<NoteCategory> NoteCategories { get; set; }
  }

  [Table("Category")]
  public class Category
  {
    public int CategoryID { get; set; }
    [Required]
    [MinLength(1), MaxLength(64)]
    public string Name { get; set; }

    public List<NoteCategory> NoteCategories { get; set; }
  }

  [Table("NoteCategory")]
  public class NoteCategory
  {
    public int NoteID { get; set; }
    public Note Note { get; set; }

    public int CategoryID { get; set; }
    public Category Category { get; set; }
  }
}
