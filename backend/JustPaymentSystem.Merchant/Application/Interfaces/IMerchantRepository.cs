using Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMerchantRepository
    {
        Task<Merchant?> GetWebhookSettingsAsync(Guid merchantId, string eventType);
        Task AddMerchantAsync(Merchant merchant);
        Task SaveChangesAsync();
    }
}
