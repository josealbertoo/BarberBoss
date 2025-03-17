using BarberBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess;
public class BarberBossDbContext : DbContext
{
    public BarberBossDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Invoice> Invoicing { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tag>().ToTable("Tags");
    }
}
