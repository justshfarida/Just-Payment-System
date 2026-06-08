using Domain.Entitites;
using Domain.Interfaces;

namespace Application.Interfaces
{
    public interface IMerchantRepository : IRepositoryBase<Merchant>
    {
        Task<Merchant?> GetWebhookSettingsAsync(Guid merchantId, string eventType);
        Task AddMerchantAsync(Merchant merchant);
        Task SaveChangesAsync();
    }
}
