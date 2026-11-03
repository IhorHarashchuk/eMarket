using MinimalWebApp.Application.Abstractions.Messaging;
using MinimalWebApp.Domain.Common;

namespace MinimalWebApp.Application.Products.Commands.CreateProduct;

/// <summary>
/// Nullable strings: this is raw, unvalidated input straight off the wire.
/// The domain decides whether it is acceptable.
/// </summary>
public sealed record CreateProductCommand(string? Sku, string? Name, string? Description, decimal Price)
    : ICommand<Result<ProductResponse>>;
