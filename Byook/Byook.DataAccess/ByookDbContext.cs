using Byook.Models;
using Microsoft.EntityFrameworkCore;

namespace Byook.DataAccess
{
    public class ByookDbContext : DbContext
    {
        public DbSet<Consumer>? Consumer { get; set; }
        public DbSet<Seller>? Seller { get; set; }
        public DbSet<Product>? Product { get; set; }
        public DbSet<Order>? Order { get; set; }

        public ByookDbContext(DbContextOptions<ByookDbContext> options) : base(options)
        {
        }
    }
}