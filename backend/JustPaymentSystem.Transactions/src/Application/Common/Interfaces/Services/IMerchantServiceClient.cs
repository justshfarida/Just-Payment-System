using Application.DTOs;

namespace Application.Common.Interfaces.Services;

public interface IMerchantServiceClient
{
    Task<MerchantPrivateKeyDto> GetKeysAsync(string MerchantId);
}
