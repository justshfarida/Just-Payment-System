using Domain.Entitites;
using Domain.Interfaces;

namespace Application.Interfaces.Repositories;

public interface IApiCredentialRepository : IRepositoryBase<ApiCredential>
{
    Task<ApiCredential?> GetByMerchantIdAsync(Guid merchantId);
    Task<ApiCredential?> GetByUserIdAsync(Guid userId);
}