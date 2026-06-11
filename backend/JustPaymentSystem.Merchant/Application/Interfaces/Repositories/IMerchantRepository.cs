using Domain.Entitites;
using Domain.Interfaces;

namespace Application.Interfaces.Repositories;

public interface IMerchantRepository : IRepositoryBase<Merchant>
{
    Task<Merchant?> GetByUserIdAsync(Guid userId);
    Task<Merchant?> GetWebhookSettingsAsync(Guid merchantId, string eventType);
    Task CreateMerchantAsync(Merchant merchant);
    Task SaveChangesAsync();
}
