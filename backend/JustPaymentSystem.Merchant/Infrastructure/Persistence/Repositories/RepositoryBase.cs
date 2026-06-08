using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected MerchantDbContext RepositoryContext;

    public RepositoryBase(MerchantDbContext repositoryContext)
            => RepositoryContext = repositoryContext;

    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ?
          RepositoryContext.Set<T>()
            .AsNoTracking() :
          RepositoryContext.Set<T>();

    public Task<T?> FindByIdAsync(Expression<Func<T, bool>> expression) =>
      RepositoryContext.Set<T>().Where(expression).FirstOrDefaultAsync();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
        !trackChanges ?
          RepositoryContext.Set<T>()
            .Where(expression)
            .AsNoTracking() :
          RepositoryContext.Set<T>()
            .Where(expression);

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression)
      => await RepositoryContext.Set<T>().AnyAsync(expression);

    public async Task CreateAsync(T entity) => await RepositoryContext.Set<T>().AddAsync(entity);

    public async Task CreateManyAsync(IEnumerable<T> entities) => await RepositoryContext.Set<IEnumerable<T>>().AddRangeAsync(entities);
    public async Task<int> CountAsync() => await RepositoryContext.Set<T>().CountAsync();
    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate) => await RepositoryContext.Set<T>().CountAsync();

    public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);

    public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);
}