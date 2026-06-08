using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMerchantService
    {
        Task<WebhookSettingsResponseDto?> GetMerchantWebhookSettingsAsync(Guid merchantId, string eventType);
        Task CreateMerchant(Guid userId, CreateMerchantDto request, CancellationToken cancellationToken);
    }
}
