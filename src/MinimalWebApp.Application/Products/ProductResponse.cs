using MinimalWebApp.Domain.Products;

namespace MinimalWebApp.Application.Products;

/// <summary>
/// Outgoing product data. Keeps the wire contract independent of the entity's shape.
/// </summary>
public sealed record ProductResponse(
    Guid Id,
    string Sku,
    string Name,
    string? Description,
    decimal Price,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc)
{
    public static ProductResponse From(Product product) => new(
        product.Id,
        product.Sku,
        product.Name,
        product.Description,
        product.Price,
        product.CreatedAtUtc,
        product.UpdatedAtUtc);
}
