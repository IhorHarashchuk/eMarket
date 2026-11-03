namespace MinimalWebApp.Application.Abstractions.Messaging;

/// <summary>
/// A request that changes state. The generic argument is what the caller gets back,
/// so the type itself declares its own response — no out-of-band contract.
/// </summary>
public interface ICommand<TResponse>;
