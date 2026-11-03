using Microsoft.EntityFrameworkCore;

namespace MinimalWebApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var product = modelBuilder.Entity<Product>();

        product.ToTable("products");
        product.HasKey(p => p.Id);

        product.Property(p => p.Sku).HasMaxLength(32).IsRequired();
        product.HasIndex(p => p.Sku).IsUnique();

        product.Property(p => p.Name).HasMaxLength(200).IsRequired();
        product.Property(p => p.Description).HasMaxLength(2000);
        product.Property(p => p.Price).HasPrecision(18, 2);
        product.Property(p => p.CreatedAtUtc).IsRequired();
    }
}
