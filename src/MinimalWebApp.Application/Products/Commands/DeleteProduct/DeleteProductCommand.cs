using MinimalWebApp.Application.Abstractions.Messaging;
using MinimalWebApp.Domain.Common;

namespace MinimalWebApp.Application.Products.Commands.DeleteProduct;

/// <summary>Returns the non-generic <see cref="Result"/>: it can fail, but produces nothing.</summary>
public sealed record DeleteProductCommand(Guid Id) : ICommand<Result>;
