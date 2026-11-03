namespace MinimalWebApp.Application.Products;

/// <summary>Incoming product data for create and update.</summary>
public sealed record ProductRequest(string? Sku, string? Name, string? Description, decimal Price);
