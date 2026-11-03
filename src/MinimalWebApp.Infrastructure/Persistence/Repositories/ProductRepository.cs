using Microsoft.EntityFrameworkCore;
using MinimalWebApp.Application.Abstractions.Persistence;
using MinimalWebApp.Domain.Products;

namespace MinimalWebApp.Infrastructure.Persistence.Repositories;

/// <summary>
/// Write side. Entities come back tracked on purpose: the handler mutates them and
/// <see cref="IUnitOfWork"/> persists the change.
/// </summary>
internal sealed class ProductRepository(AppDbContext db) : IProductRepository
{
    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        db.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public Task<bool> SkuExistsAsync(string sku, Guid? excludeId = null, CancellationToken cancellationToken = default) =>
        db.Products.AnyAsync(p => p.Sku == sku && (excludeId == null || p.Id != excludeId), cancellationToken);

    public void Add(Product product) => db.Products.Add(product);

    public void Remove(Product product) => db.Products.Remove(product);
}
