using Microsoft.EntityFrameworkCore;
using Z01.Models.Category;

namespace Z01.Models
{
    public class NoteCategoryContext : DbContext
    {
        public NoteCategoryContext(DbContextOptions<NoteCategoryContext> options)
            : base(options)
        {
        }

        public DbSet<NoteCategory> NoteCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
 
            modelBuilder.Entity<NoteCategory>()
                .HasKey(bc => new {bc.NoteId, bc.CategoryId});
            modelBuilder.Entity<NoteCategory>()
                .HasOne(bc => bc.Note)
                .WithMany(b => b.NoteCategories)
                .HasForeignKey(bc => bc.NoteId);
            modelBuilder.Entity<NoteCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.NoteCategories)
                .HasForeignKey(bc => bc.CategoryId);
        }
    }
}
