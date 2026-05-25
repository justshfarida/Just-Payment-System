using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T>
    where T : class
{
    protected readonly TransactionDbContext _db;

    public RepositoryBase(TransactionDbContext db)
    {
        _db = db;
    }

    public Task<int> CountAsync(Expression<Func<T, bool>> predicate) => _db.Set<T>()
            .CountAsync(predicate);

    public void Delete(T entity) => _db.Set<T>().Remove(entity);

    public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) => _db.Set<T>().AnyAsync(predicate);

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate, bool track = false, CancellationToken cancellationToken = default) => !track ?
          _db.Set<T>()
            .Where(predicate)
            .AsNoTracking() :
          _db.Set<T>()
            .Where(predicate);

    public async Task InsertAsync(T transaction, CancellationToken cancellationToken = default) => await _db.Set<T>().AddAsync(transaction);
}
