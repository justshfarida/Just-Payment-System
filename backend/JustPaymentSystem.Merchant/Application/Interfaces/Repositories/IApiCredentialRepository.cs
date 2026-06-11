using Domain.Entitites;
using Domain.Interfaces;

namespace Application.Interfaces.Repositories;

public interface IApiCredentialRepository : IRepositoryBase<ApiCredential>
{
    Task<ApiCredential?> GetByMerchantIdAsync(Guid merchantId, bool trackChanges = false);
    Task<ApiCredential?> GetByUserIdAsync(Guid userId, bool trackChanges = false);
    Task SaveChangesAsync();
}
