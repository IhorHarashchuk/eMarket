using MinimalWebApp.Application.Abstractions.Messaging;
using MinimalWebApp.Application.Abstractions.Persistence;
using MinimalWebApp.Domain.Common;
using MinimalWebApp.Domain.Products;

namespace MinimalWebApp.Application.Products.Commands.CreateProduct;

internal sealed class CreateProductCommandHandler(
    IProductRepository products,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) : ICommandHandler<CreateProductCommand, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(
        CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var created = Product.Create(
            command.Sku,
            command.Name,
            command.Description,
            command.Price,
            timeProvider.GetUtcNow());

        if (created.IsFailure)
        {
            return Result.Failure<ProductResponse>(created.Error!);
        }

        var product = created.Value;

        // Uniqueness spans all products, so the entity cannot check it — this is the
        // one rule that needs storage and therefore belongs to the use case.
        if (await products.SkuExistsAsync(product.Sku, cancellationToken: cancellationToken))
        {
            return Result.Failure<ProductResponse>(ProductErrors.DuplicateSku(product.Sku));
        }

        products.Add(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(ProductResponse.From(product));
    }
}
