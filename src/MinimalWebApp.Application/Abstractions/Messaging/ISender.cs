namespace MinimalWebApp.Application.Abstractions.Messaging;

/// <summary>
/// Hands a message to its registered handler. Callers depend on this one interface
/// instead of on every handler they need.
/// </summary>
public interface ISender
{
    Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);

    Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
}
