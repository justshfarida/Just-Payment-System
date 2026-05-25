using Domain.Domains;

namespace Application.Common.Interfaces;

public interface ITransactionRepository : IRepositoryBase<Transaction>
{
    public Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
