using MinimalWebApp.Application.Abstractions.Messaging;
using MinimalWebApp.Application.Abstractions.Persistence;
using MinimalWebApp.Domain.Common;
using MinimalWebApp.Domain.Products;

namespace MinimalWebApp.Application.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler(
    IProductRepository products,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) : ICommandHandler<UpdateProductCommand, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(
        UpdateProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = await products.GetByIdAsync(command.Id, cancellationToken);
        if (product is null)
        {
            return Result.Failure<ProductResponse>(ProductErrors.NotFound(command.Id));
        }

        // Validate and check uniqueness before mutating, so a rejected update
        // leaves the tracked entity untouched.
        var valid = Product.Check(command.Sku, command.Name, command.Description, command.Price);
        if (valid.IsFailure)
        {
            return Result.Failure<ProductResponse>(valid.Error!);
        }

        var sku = Product.NormalizeSku(command.Sku!);
        if (await products.SkuExistsAsync(sku, command.Id, cancellationToken))
        {
            return Result.Failure<ProductResponse>(ProductErrors.DuplicateSku(sku));
        }

        product.Update(
            command.Sku,
            command.Name,
            command.Description,
            command.Price,
            timeProvider.GetUtcNow());

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(ProductResponse.From(product));
    }
}
