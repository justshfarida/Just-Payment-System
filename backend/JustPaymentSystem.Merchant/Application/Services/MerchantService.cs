using Application.Dtos;
using Application.Interfaces.MappingProfiles;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;

namespace Application.Services;

public class MerchantService : IMerchantService
{
    private readonly IMerchantRepository _repository;
    private readonly IMerchantMapper _mapper;


    public MerchantService(IMerchantRepository repository, IMerchantMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
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

    public async Task CreateMerchant(Guid userId, CreateMerchantDto request, CancellationToken cancellationToken)
    {
        var isUserAlreadyMerchant = await _repository.ExistsAsync(m => m.UserId == userId);
        if (isUserAlreadyMerchant)
        {
            throw new Exception("User is already a merchant.");
        }

        var isEmailDuplicate = await _repository.
                ExistsAsync(m => m.Contact != null && m.Contact.Email == request.Email);
        var isPhoneDuplicate = await _repository.
                ExistsAsync(m => m.Contact != null && m.Contact.Phone == request.PhoneNumber);
        var isVOENDuplicate = await _repository.
                ExistsAsync(m => m.VOEN == request.VOEN);
        if (isEmailDuplicate || isPhoneDuplicate)
        {
            throw new Exception("Email or phone number already exists.");
        }

        if (isVOENDuplicate)
        {
            throw new Exception("VOEN already exists.");
        }

        var merchant = _mapper.Map(request, userId);
        await _repository.CreateMerchantAsync(merchant);
    }
}
