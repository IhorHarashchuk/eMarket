using MinimalWebApp.Application.Abstractions.Messaging;

namespace MinimalWebApp.Application.Products.Queries.ListProducts;

/// <summary>No parameters yet — paging and filtering would be added here.</summary>
public sealed record ListProductsQuery : IQuery<IReadOnlyList<ProductResponse>>;
