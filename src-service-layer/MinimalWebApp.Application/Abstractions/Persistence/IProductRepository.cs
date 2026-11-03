using MinimalWebApp.Domain.Products;

namespace MinimalWebApp.Application.Abstractions.Persistence;

/// <summary>
/// Storage for <see cref="Product"/> aggregates. Implemented in the infrastructure layer.
/// </summary>
public interface IProductRepository
{
    Task<IReadOnlyList<Product>> ListAsync(CancellationToken cancellationToken = default);

    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <param name="excludeId">Id to ignore, so an update does not collide with itself.</param>
    Task<bool> SkuExistsAsync(string sku, Guid? excludeId = null, CancellationToken cancellationToken = default);

    void Add(Product product);

    void Remove(Product product);
}
