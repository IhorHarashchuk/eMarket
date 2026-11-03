using MinimalWebApp.Application.Products;

namespace MinimalWebApp.Application.Abstractions.Persistence;

/// <summary>
/// The READ side. Returns the response contract directly, never an entity: the
/// implementation projects straight from the database and skips change tracking.
/// No <see cref="IUnitOfWork"/> is involved — nothing here can write.
/// </summary>
public interface IProductReader
{
    Task<IReadOnlyList<ProductResponse>> ListAsync(CancellationToken cancellationToken = default);

    Task<ProductResponse?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
