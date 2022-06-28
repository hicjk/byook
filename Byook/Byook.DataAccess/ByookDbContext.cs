using Byook.Models;
using Microsoft.EntityFrameworkCore;

namespace Byook.DataAccess;

public class ByookDbContext : DbContext
{
    public DbSet<Consumer>? Consumers { get; set; }
    public DbSet<Seller>? Sellers { get; set; }

    public ByookDbContext(DbContextOptions<ByookDbContext> options) : base(options)
    {
    }
}