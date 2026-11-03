namespace MinimalWebApp.Application.Abstractions.Persistence;

/// <summary>
/// Commits the changes tracked across repositories as a single transaction.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
