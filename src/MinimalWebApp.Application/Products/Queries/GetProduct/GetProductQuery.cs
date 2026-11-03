using MinimalWebApp.Application.Abstractions.Messaging;
using MinimalWebApp.Domain.Common;

namespace MinimalWebApp.Application.Products.Queries.GetProduct;

public sealed record GetProductQuery(Guid Id) : IQuery<Result<ProductResponse>>;
