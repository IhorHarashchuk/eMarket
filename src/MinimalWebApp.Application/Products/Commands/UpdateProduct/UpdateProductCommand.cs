using MinimalWebApp.Application.Abstractions.Messaging;
using MinimalWebApp.Domain.Common;

namespace MinimalWebApp.Application.Products.Commands.UpdateProduct;

/// <summary>
/// <paramref name="Id"/> is supplied by the route, not the request body —
/// see how the endpoint overwrites it.
/// </summary>
public sealed record UpdateProductCommand(Guid Id, string? Sku, string? Name, string? Description, decimal Price)
    : ICommand<Result<ProductResponse>>;
