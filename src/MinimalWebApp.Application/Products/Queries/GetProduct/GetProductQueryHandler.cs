using MinimalWebApp.Application.Abstractions.Messaging;
using MinimalWebApp.Application.Abstractions.Persistence;
using MinimalWebApp.Domain.Common;
using MinimalWebApp.Domain.Products;

namespace MinimalWebApp.Application.Products.Queries.GetProduct;

internal sealed class GetProductQueryHandler(IProductReader products)
    : IQueryHandler<GetProductQuery, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var product = await products.GetAsync(query.Id, cancellationToken);

        return product is null
            ? Result.Failure<ProductResponse>(ProductErrors.NotFound(query.Id))
            : Result.Success(product);
    }
}
