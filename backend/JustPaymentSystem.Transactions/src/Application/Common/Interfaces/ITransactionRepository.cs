using Domain.Domains;
using Domain.Shared.Enums;

namespace Application.Common.Interfaces;

public interface ITransactionRepository : IRepositoryBase<Transaction>
{
    public Task<List<Transaction>> GetAllAsync(int page, int pageSize, string? merchantId = null, TransactionStatus? status = null, CancellationToken cancellationToken = default);
    public Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
