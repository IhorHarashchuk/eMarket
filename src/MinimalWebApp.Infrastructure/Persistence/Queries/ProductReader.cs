using Microsoft.EntityFrameworkCore;
using MinimalWebApp.Application.Abstractions.Persistence;
using MinimalWebApp.Application.Products;

namespace MinimalWebApp.Infrastructure.Persistence.Queries;

/// <summary>
/// Read side. Two deliberate differences from the repository:
/// <list type="bullet">
/// <item>the projection is written inline, so SQL selects only the response's columns;</item>
/// <item><c>AsNoTracking</c> skips the change tracker, which a read never needs.</item>
/// </list>
/// The entity is never materialised, which is exactly the point of splitting reads
/// from writes — and why <c>ProductResponse.From</c> cannot be used here: EF has to
/// translate the projection to SQL, and it cannot translate a method call.
/// </summary>
internal sealed class ProductReader(AppDbContext db) : IProductReader
{
    public async Task<IReadOnlyList<ProductResponse>> ListAsync(CancellationToken cancellationToken = default) =>
        await db.Products
            .AsNoTracking()
            .OrderBy(p => p.Sku)
            .Select(p => new ProductResponse(
                p.Id, p.Sku, p.Name, p.Description, p.Price, p.CreatedAtUtc, p.UpdatedAtUtc))
            .ToListAsync(cancellationToken);

    public Task<ProductResponse?> GetAsync(Guid id, CancellationToken cancellationToken = default) =>
        db.Products
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductResponse(
                p.Id, p.Sku, p.Name, p.Description, p.Price, p.CreatedAtUtc, p.UpdatedAtUtc))
            .FirstOrDefaultAsync(cancellationToken);
}
