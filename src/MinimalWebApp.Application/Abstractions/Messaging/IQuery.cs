namespace MinimalWebApp.Application.Abstractions.Messaging;

/// <summary>
/// A request that only reads. Separate from <see cref="ICommand{TResponse}"/> so the
/// read path can take shortcuts a write path must not (no tracking, no aggregates).
/// </summary>
public interface IQuery<TResponse>;
