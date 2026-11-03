using MinimalWebApp.Application.Abstractions.Messaging;
using MinimalWebApp.Application.Abstractions.Persistence;

namespace MinimalWebApp.Application.Products.Queries.ListProducts;

/// <summary>
/// Note what is absent: no <see cref="IUnitOfWork"/>, no entity, no <c>Result</c>.
/// Listing cannot fail for a business reason, so the signature does not pretend it can.
/// </summary>
internal sealed class ListProductsQueryHandler(IProductReader products)
    : IQueryHandler<ListProductsQuery, IReadOnlyList<ProductResponse>>
{
    public Task<IReadOnlyList<ProductResponse>> Handle(
        ListProductsQuery query,
        CancellationToken cancellationToken) =>
        products.ListAsync(cancellationToken);
}
