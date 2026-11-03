using MinimalWebApp.Application.Abstractions.Messaging;
using MinimalWebApp.Application.Abstractions.Persistence;
using MinimalWebApp.Domain.Common;
using MinimalWebApp.Domain.Products;

namespace MinimalWebApp.Application.Products.Commands.DeleteProduct;

internal sealed class DeleteProductCommandHandler(
    IProductRepository products,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteProductCommand, Result>
{
    public async Task<Result> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await products.GetByIdAsync(command.Id, cancellationToken);
        if (product is null)
        {
            return Result.Failure(ProductErrors.NotFound(command.Id));
        }

        products.Remove(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
