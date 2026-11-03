using MinimalWebApp.Application.Abstractions.Persistence;
using MinimalWebApp.Domain.Common;
using MinimalWebApp.Domain.Products;

namespace MinimalWebApp.Application.Products;

public sealed class ProductService(
    IProductRepository products,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) : IProductService
{
    public async Task<IReadOnlyList<ProductResponse>> ListAsync(CancellationToken cancellationToken = default)
    {
        var found = await products.ListAsync(cancellationToken);
        return [.. found.Select(ProductResponse.From)];
    }

    public async Task<Result<ProductResponse>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await products.GetByIdAsync(id, cancellationToken);

        return product is null
            ? Result.Failure<ProductResponse>(ProductErrors.NotFound(id))
            : Result.Success(ProductResponse.From(product));
    }

    public async Task<Result<ProductResponse>> CreateAsync(
        ProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var created = Product.Create(
            request.Sku,
            request.Name,
            request.Description,
            request.Price,
            timeProvider.GetUtcNow());

        if (created.IsFailure)
        {
            return Result.Failure<ProductResponse>(created.Error!);
        }

        var product = created.Value;

        if (await products.SkuExistsAsync(product.Sku, cancellationToken: cancellationToken))
        {
            return Result.Failure<ProductResponse>(ProductErrors.DuplicateSku(product.Sku));
        }

        products.Add(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(ProductResponse.From(product));
    }

    public async Task<Result<ProductResponse>> UpdateAsync(
        Guid id,
        ProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var product = await products.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return Result.Failure<ProductResponse>(ProductErrors.NotFound(id));
        }

        // Validate and check uniqueness before mutating, so a rejected update
        // leaves the tracked entity untouched.
        var valid = Product.Check(request.Sku, request.Name, request.Description, request.Price);
        if (valid.IsFailure)
        {
            return Result.Failure<ProductResponse>(valid.Error!);
        }

        var sku = Product.NormalizeSku(request.Sku!);
        if (await products.SkuExistsAsync(sku, id, cancellationToken))
        {
            return Result.Failure<ProductResponse>(ProductErrors.DuplicateSku(sku));
        }

        product.Update(
            request.Sku,
            request.Name,
            request.Description,
            request.Price,
            timeProvider.GetUtcNow());

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(ProductResponse.From(product));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await products.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return Result.Failure(ProductErrors.NotFound(id));
        }

        products.Remove(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
