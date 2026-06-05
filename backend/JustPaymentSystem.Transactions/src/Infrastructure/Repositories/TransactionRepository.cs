using Application.Common.Interfaces;
using Domain.Domains;
using Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
{
    public TransactionRepository(TransactionDbContext context) : base(context)
    {
    }

    public Task<List<Transaction>> GetAllAsync(int page, int pageSize, string? merchantId = null, TransactionStatus? status = null, CancellationToken cancellationToken = default)
    {
        return _db.Transactions
            .Where(c => merchantId == null || c.MerchantId == merchantId)
            .Where(c => status == null || c.Status == status)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .OrderByDescending(c => c.UpdatedAt)
                .ThenByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return FindByCondition(c => c.Id == id, true)
            .Include(c => c.PaymentSnapshot)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
