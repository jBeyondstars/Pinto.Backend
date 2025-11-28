using Microsoft.EntityFrameworkCore;
using Pinto.Domain.Entities;

namespace Pinto.Infrastructure.Persistence;

public class PintoDbContext : DbContext
{
    public PintoDbContext(DbContextOptions<PintoDbContext> options) : base(options)
    {
    }

    public DbSet<Board> Boards => Set<Board>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Board>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.CanvasData).HasColumnType("jsonb");
            entity.HasIndex(e => e.OwnerId);
        });
    }
}
