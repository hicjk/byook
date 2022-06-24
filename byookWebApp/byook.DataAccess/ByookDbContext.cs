using byook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace byook.DataAccess;

public class ByookDbContext : IdentityDbContext
{
    public DbSet<Consumer>? Consumers { get; set; }
    public DbSet<Seller>? Sellers { get; set; }

    public ByookDbContext(DbContextOptions<ByookDbContext> options) : base(options)
    {
    }
}