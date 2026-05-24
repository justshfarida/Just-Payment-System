using System.Linq.Expressions;

namespace Application.Common.Interfaces;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate, bool track = false, CancellationToken cancellationToken = default);
    Task InsertAsync(T entity, CancellationToken cancellationToken = default);
    void Delete(T entity);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);

}
