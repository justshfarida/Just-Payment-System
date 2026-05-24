using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Domains;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
{
    public TransactionRepository(TransactionDbContext context) : base(context)
    {
    }

    public Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _db.Transactions
            .Include(c => c.PaymentSnapshot)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
