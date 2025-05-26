using Productos.Models;
using Microsoft.EntityFrameworkCore;
using Productos.Models;

namespace Productos.Context
{
    public class dbContext : DbContext
    {
        public dbContext(DbContextOptions<dbContext> options) : base(options) { }
        public DbSet<Producto> Productos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); modelBuilder.Entity<Producto>().HasKey(u => u.Id);
        }
    }
}
