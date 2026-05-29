using Application.Dtos;
using Application.Interfaces;

namespace Application.Services;

public class MerchantService : IMerchantService
{
    private readonly IMerchantRepository _repository;

    public MerchantService(IMerchantRepository repository)
    {
        _repository = repository;
    }

    public async Task<WebhookSettingsResponseDto?> GetMerchantWebhookSettingsAsync(Guid merchantId, string eventType)
    {
        var merchant = await _repository.GetWebhookSettingsAsync(merchantId, eventType);

        if (merchant?.Webhook == null) return null;

        //var isSubscribed = merchant.Webhook.WebhookEvents?
        //    .Any(we => we.Event?.Name.Equals(eventType, StringComparison.OrdinalIgnoreCase) ?? false) ?? false;

        //if (!isSubscribed) return null;

        return new WebhookSettingsResponseDto
        {
            WebhookUrl = merchant.Webhook.WebhookUrl,
            SecretKey = merchant.Webhook.SecretKey
        };
    }
}