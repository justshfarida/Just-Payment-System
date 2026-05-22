using Application.Common.Interfaces;

namespace Infrastructure.Repositories;

public class UnitOfWork(TransactionDbContext db) : IUnitOfWork
{
    public Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        return db.SaveChangesAsync(cancellationToken);
    }
}
