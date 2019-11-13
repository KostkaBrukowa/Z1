using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Z01.Models.Category;

namespace Z01.Models
{
    public class CategoryContext : DbContext
    {
        public CategoryContext(DbContextOptions<CategoryContext> options)
            : base(options)
        {
        }

        public DbSet<CategoryModel> CategoryItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory) //tie-up DbContext with LoggerFactory object
                .EnableSensitiveDataLogging();

        } 

        public static readonly ILoggerFactory loggerFactory = new LoggerFactory(new[] {
            new ConsoleLoggerProvider((_, __) => true, true)
        }); 
    }
}
