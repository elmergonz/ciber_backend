using Microsoft.EntityFrameworkCore;
using ciber_backend.Models;

namespace ciber_backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}
