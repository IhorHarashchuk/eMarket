namespace MinimalWebApp.Application.Abstractions.Messaging;

/// <inheritdoc cref="ICommandHandler{TCommand,TResponse}"/>
public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);
}
