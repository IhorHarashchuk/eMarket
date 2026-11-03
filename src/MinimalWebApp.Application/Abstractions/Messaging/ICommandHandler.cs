namespace MinimalWebApp.Application.Abstractions.Messaging;

/// <summary>
/// Executes exactly one <see cref="ICommand{TResponse}"/>. One handler per command,
/// one public method — the whole use case lives here and nowhere else.
/// </summary>
public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
}
