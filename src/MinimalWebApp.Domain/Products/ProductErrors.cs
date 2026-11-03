using MinimalWebApp.Domain.Common;

namespace MinimalWebApp.Domain.Products;

public static class ProductErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("product.not_found", $"No product with id '{id}' exists.");

    public static Error DuplicateSku(string sku) =>
        Error.Conflict("product.duplicate_sku", $"A product with SKU '{sku}' already exists.");
}
