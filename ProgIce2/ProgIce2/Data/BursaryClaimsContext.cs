using Microsoft.EntityFrameworkCore;
using ProgIce2.Models;

namespace ProgIce2.Data;

public class BursaryClaimsContext(DbContextOptions<BursaryClaimsContext> options) : DbContext(options)
{
    public DbSet<Claim> Claims => Set<Claim>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Claim>(entity =>
        {
            entity.HasKey(c => c.ClaimId);
            entity.Property(c => c.StudentNumber).HasMaxLength(20).IsRequired();
            entity.Property(c => c.FirstName).HasMaxLength(50).IsRequired();
            entity.Property(c => c.Surname).HasMaxLength(50).IsRequired();
            entity.Property(c => c.HoursWorked).HasPrecision(5, 2);
            entity.Property(c => c.HourlyRate).HasPrecision(10, 2);
            entity.Property(c => c.TotalClaimAmount).HasPrecision(10, 2);
            entity.Property(c => c.Status).HasConversion<string>().HasMaxLength(20);
        });
    }
}
