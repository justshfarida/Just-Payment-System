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

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await _db.Transactions.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if(transaction == null)
        {
            throw new NotFoundException($"No transaction with {id}");
        }
        _db.Transactions.Remove(transaction);
    }

    public Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _db.Transactions
            .Include(c => c.PaymentSnapshot)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task InsertAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        await _db.Transactions.AddAsync(transaction);
    }
}
