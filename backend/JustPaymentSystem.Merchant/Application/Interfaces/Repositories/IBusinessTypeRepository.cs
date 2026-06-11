using Domain.Entitites;
using Domain.Interfaces;

namespace Application.Interfaces.Repositories;

public interface IBusinessTypeRepository : IRepositoryBase<BusinessType>
{
    Task<IReadOnlyList<BusinessType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BusinessType?> GetByIdAsync(Guid id, bool trackChanges = false);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, Guid excludeId, CancellationToken cancellationToken = default);
    Task<bool> HasMerchantsAsync(Guid businessTypeId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync();
}
