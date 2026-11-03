using MinimalWebApp.Domain.Common;

namespace MinimalWebApp.Domain.Products;

/// <summary>
/// A sellable product. Invariants (shape of the SKU, field lengths, non-negative price)
/// are enforced here so no caller can build an invalid instance.
/// </summary>
public class Product
{
    public const int SkuMaxLength = 32;
    public const int NameMaxLength = 200;
    public const int DescriptionMaxLength = 2000;

    // Required by EF Core's materializer.
    private Product()
    {
    }

    private Product(Guid id, string sku, string name, string? description, decimal price, DateTimeOffset createdAtUtc)
    {
        Id = id;
        Sku = sku;
        Name = name;
        Description = description;
        Price = price;
        CreatedAtUtc = createdAtUtc;
    }

    public Guid Id { get; private set; }

    public string Sku { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string? Description { get; private set; }

    public decimal Price { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset? UpdatedAtUtc { get; private set; }

    public static Result<Product> Create(
        string? sku,
        string? name,
        string? description,
        decimal price,
        DateTimeOffset nowUtc)
    {
        if (Validate(sku, name, description, price) is { } error)
        {
            return Result.Failure<Product>(error);
        }

        return Result.Success(new Product(
            Guid.CreateVersion7(),
            NormalizeSku(sku!),
            name!.Trim(),
            NormalizeDescription(description),
            decimal.Round(price, 2),
            nowUtc));
    }

    public Result Update(
        string? sku,
        string? name,
        string? description,
        decimal price,
        DateTimeOffset nowUtc)
    {
        if (Validate(sku, name, description, price) is { } error)
        {
            return Result.Failure(error);
        }

        Sku = NormalizeSku(sku!);
        Name = name!.Trim();
        Description = NormalizeDescription(description);
        Price = decimal.Round(price, 2);
        UpdatedAtUtc = nowUtc;

        return Result.Success();
    }

    /// <summary>
    /// Checks the same invariants <see cref="Create"/> and <see cref="Update"/> enforce,
    /// without producing or mutating an entity.
    /// </summary>
    public static Result Check(string? sku, string? name, string? description, decimal price) =>
        Validate(sku, name, description, price) is { } error
            ? Result.Failure(error)
            : Result.Success();

    /// <summary>
    /// Canonical form of a SKU. Uniqueness is checked against this value, so callers
    /// comparing against stored data must normalize first.
    /// </summary>
    public static string NormalizeSku(string sku) => sku.Trim().ToUpperInvariant();

    private static string? NormalizeDescription(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static Error? Validate(string? sku, string? name, string? description, decimal price)
    {
        var failures = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(sku))
        {
            failures["sku"] = ["SKU is required."];
        }
        else if (sku.Trim().Length > SkuMaxLength)
        {
            failures["sku"] = [$"SKU must be {SkuMaxLength} characters or fewer."];
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            failures["name"] = ["Name is required."];
        }
        else if (name.Trim().Length > NameMaxLength)
        {
            failures["name"] = [$"Name must be {NameMaxLength} characters or fewer."];
        }

        if (description?.Trim().Length > DescriptionMaxLength)
        {
            failures["description"] = [$"Description must be {DescriptionMaxLength} characters or fewer."];
        }

        if (price < 0m)
        {
            failures["price"] = ["Price cannot be negative."];
        }

        return failures.Count > 0 ? Error.Validation(failures) : null;
    }
}
