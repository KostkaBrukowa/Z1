using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                .HasKey(t => new {NoteId = t.NoteID, CategoryId = t.CategoryID});

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

    public class Note
    {
        public string NoteID { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public bool Markdown { get; set; }
        [Required]
        [MinLength(3)]
        public string Title { get; set; }
        public string Content { get; set; }

        public List<NoteCategory> NoteCategories { get; set; }
    }

    public class Category
    {
        public string CategoryID { get; set; }
        public string Name { get; set; }

        public List<NoteCategory> NoteCategories { get; set; }
    }

    public class NoteCategory
    {
        public string NoteID { get; set; }
        public Note Note { get; set; }

        public string CategoryID { get; set; }
        public Category Category { get; set; }
    }
}
