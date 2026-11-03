using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalWebApp.Domain.Products;

namespace MinimalWebApp.Infrastructure.Persistence.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> product)
    {
        product.ToTable("products");
        product.HasKey(p => p.Id);

        product.Property(p => p.Sku).HasMaxLength(Product.SkuMaxLength).IsRequired();
        product.HasIndex(p => p.Sku).IsUnique();

        product.Property(p => p.Name).HasMaxLength(Product.NameMaxLength).IsRequired();
        product.Property(p => p.Description).HasMaxLength(Product.DescriptionMaxLength);
        product.Property(p => p.Price).HasPrecision(18, 2);
        product.Property(p => p.CreatedAtUtc).IsRequired();
    }
}
