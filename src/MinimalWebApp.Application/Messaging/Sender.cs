using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MinimalWebApp.Application.Abstractions.Messaging;

namespace MinimalWebApp.Application.Messaging;

/// <summary>
/// Closes the open handler interface over the message's concrete type, pulls that
/// handler out of DI and invokes it. Reflection is needed because the caller only
/// knows the message as <c>ICommand&lt;TResponse&gt;</c>, while the handler is
/// registered against the concrete command type.
/// </summary>
internal sealed class Sender(IServiceProvider provider) : ISender
{
    // MethodInfo lookup is the slow part of reflection; the result never changes per type.
    private static readonly ConcurrentDictionary<Type, MethodInfo> HandleMethods = new();

    public Task<TResponse> Send<TResponse>(
        ICommand<TResponse> command,
        CancellationToken cancellationToken = default) =>
        Dispatch<TResponse>(typeof(ICommandHandler<,>), command, cancellationToken);

    public Task<TResponse> Send<TResponse>(
        IQuery<TResponse> query,
        CancellationToken cancellationToken = default) =>
        Dispatch<TResponse>(typeof(IQueryHandler<,>), query, cancellationToken);

    private Task<TResponse> Dispatch<TResponse>(
        Type openHandlerType,
        object message,
        CancellationToken cancellationToken)
    {
        // ICommandHandler<,> + CreateProductCommand + Result<ProductResponse>
        //   -> ICommandHandler<CreateProductCommand, Result<ProductResponse>>
        var handlerType = openHandlerType.MakeGenericType(message.GetType(), typeof(TResponse));

        // Throws if no handler is registered — a wiring bug should fail loudly.
        var handler = provider.GetRequiredService(handlerType);
        var handle = HandleMethods.GetOrAdd(handlerType, static type => type.GetMethod("Handle")!);

        return (Task<TResponse>)handle.Invoke(handler, [message, cancellationToken])!;
    }
}
