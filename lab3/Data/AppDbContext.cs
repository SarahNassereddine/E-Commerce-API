using lab3.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product>   Products { get; set; }

    }
}
