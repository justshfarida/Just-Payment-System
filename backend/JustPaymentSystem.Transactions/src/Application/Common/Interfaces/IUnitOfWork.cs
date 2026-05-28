namespace Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
