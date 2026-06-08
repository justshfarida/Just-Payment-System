using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindAll(bool trackChanges);
    Task<T?> FindByIdAsync(Expression<Func<T, bool>> expression);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
    Task CreateAsync(T entity);
    Task CreateManyAsync(IEnumerable<T> entities);
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate);
    void Update(T entity);
    void Delete(T entity);
}
