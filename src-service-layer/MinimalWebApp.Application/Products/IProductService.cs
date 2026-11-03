using MinimalWebApp.Domain.Common;

namespace MinimalWebApp.Application.Products;

/// <summary>The product use cases, free of any transport concern.</summary>
public interface IProductService
{
    Task<IReadOnlyList<ProductResponse>> ListAsync(CancellationToken cancellationToken = default);

    Task<Result<ProductResponse>> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<ProductResponse>> CreateAsync(ProductRequest request, CancellationToken cancellationToken = default);

    Task<Result<ProductResponse>> UpdateAsync(Guid id, ProductRequest request, CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
