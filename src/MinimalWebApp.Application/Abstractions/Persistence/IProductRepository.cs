using MinimalWebApp.Domain.Products;

namespace MinimalWebApp.Application.Abstractions.Persistence;

/// <summary>
/// The WRITE side. Loads and stores whole <see cref="Product"/> aggregates, because a
/// change has to go through the entity's invariants. Queries must not come through here:
/// materialising an aggregate just to read three columns is wasted work, and the
/// change tracker it loads into is pure overhead for a read.
/// </summary>
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <param name="excludeId">Id to ignore, so an update does not collide with itself.</param>
    Task<bool> SkuExistsAsync(string sku, Guid? excludeId = null, CancellationToken cancellationToken = default);

    void Add(Product product);

    void Remove(Product product);
}
